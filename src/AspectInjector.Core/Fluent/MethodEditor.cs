﻿using AspectInjector.Core.Extensions;
using AspectInjector.Core.Fluent.Models;
using AspectInjector.Core.Models;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Linq;

namespace AspectInjector.Core.Fluent
{
    public class MethodEditor
    {
        private readonly MethodDefinition _md;
        private readonly ExtendedTypeSystem _typeSystem;

        internal MethodEditor(MethodDefinition md)
        {
            _md = md;
            _typeSystem = md.Module.GetTypeSystem();
        }

        public void OnInit(Action<PointCut> action)
        {
            var instruction = _md.IsConstructor && !_md.IsStatic ?
                FindBaseClassCtorCall() :
                GetMethodOriginalEntryPoint();

            var proc = _md.Body.GetEditor();

            if (instruction.OpCode != OpCodes.Nop) //add nop
                instruction = proc.SafeInsertBefore(instruction, proc.Create(OpCodes.Nop));

            action(new PointCut(proc, instruction));
        }

        public void OnEntry(Action<PointCut> action)
        {
            var instruction = _md.IsConstructor && !_md.IsStatic ?
                FindBaseClassCtorCall() :
                GetMethodOriginalEntryPoint();

            while (instruction.OpCode == OpCodes.Nop) //skip all nops
                instruction = instruction.Next;

            action(new PointCut(_md.Body.GetEditor(), instruction));
        }

        public void OnExit(Action<PointCut> action)
        {
        }

        public void OnException(Action<PointCut> action)
        {
        }

        public void OnInstruction(Instruction instruction, Action<PointCut> action)
        {
            if (!_md.Body.Instructions.Contains(instruction))
                throw new ArgumentException("Wrong instruction.");

            action(new PointCut(_md.Body.GetEditor(), instruction));
        }

        public void Instead(Action<PointCut> action)
        {
            var proc = _md.Body.GetEditor();
            var instruction = proc.Create(OpCodes.Nop);

            _md.Body.Instructions.Clear();
            proc.Append(instruction);

            OnInstruction(instruction, action);

            proc.Remove(instruction);
        }

        protected Instruction FindBaseClassCtorCall()
        {
            var proc = _md.Body.GetEditor();

            if (!_md.IsConstructor)
                throw new Exception(_md.ToString() + " is not ctor.");

            if (_md.DeclaringType.IsValueType)
                return _md.Body.Instructions.First();

            var point = _md.Body.Instructions.FirstOrDefault(
                i => i != null && i.OpCode == OpCodes.Call && i.Operand is MethodReference
                    && ((MethodReference)i.Operand).Resolve().IsConstructor
                    && (((MethodReference)i.Operand).DeclaringType.IsTypeOf(_md.DeclaringType.BaseType)
                        || ((MethodReference)i.Operand).DeclaringType.IsTypeOf(_md.DeclaringType)));

            if (point == null)
                throw new Exception("Cannot find base class ctor call");

            return point.Next;
        }

        protected Instruction GetMethodOriginalEntryPoint()
        {
            return _md.Body.Instructions.First();
        }

        protected void Mark<T>() where T : Attribute
        {
            if (_md.CustomAttributes.Any(ca => ca.AttributeType.IsTypeOf(typeof(T))))
                return;

            var constructor = _typeSystem.CompilerGeneratedAttribute.Resolve()
                .Methods.First(m => m.IsConstructor && !m.IsStatic);

            _md.CustomAttributes.Add(new CustomAttribute(_typeSystem.Import(constructor)));
        }
    }
}