﻿using AspectInjector.Tests.Assets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AspectInjector.Tests.Runtime.Around
{
    [TestClass]
    public class TargetsInstanceTests : TestRunner
    {
        [TestMethod]
        public void AdviceAround_Instance_Setter()
        {
            ExecSetter();
            CheckSequence(new List<string> {
                InstanceAspect.AroundEnter,

                Events.TestPropertySetterExit,
                InstanceAspect.AroundExit
            });
        }

        [TestMethod]
        public void AdviceAround_Instance_Static_Setter()
        {
            ExecStaticSetter();
            CheckSequence(new List<string> {
                InstanceAspect.AroundEnter,

                Events.TestStaticPropertySetterExit,
                InstanceAspect.AroundExit
            });
        }

        [TestMethod]
        public void AdviceAround_Instance_Getter()
        {
            ExecGetter();
            CheckSequence(new List<string> {
                InstanceAspect.AroundEnter,

                Events.TestPropertyGetterExit,
                InstanceAspect.AroundExit
            });
        }

        [TestMethod]
        public void AdviceAround_Instance_Static_Getter()
        {
            ExecStaticGetter();
            CheckSequence(new List<string> {
                InstanceAspect.AroundEnter,

                Events.TestStaticPropertyGetterExit,
                InstanceAspect.AroundExit
            });
        }

        [TestMethod]
        public void AdviceAround_Instance_Add()
        {
            ExecAdd();
            CheckSequence(new List<string> {
                InstanceAspect.AroundEnter,

                Events.TestEventAddExit,
                InstanceAspect.AroundExit
            });
        }

        [TestMethod]
        public void AdviceAround_Instance_Static_Add()
        {
            ExecStaticAdd();
            CheckSequence(new List<string> {
                InstanceAspect.AroundEnter,

                Events.TestStaticEventAddExit,
                InstanceAspect.AroundExit
            });
        }

        [TestMethod]
        public void AdviceAround_Instance_Remove()
        {
            ExecRemove();
            CheckSequence(new List<string> {
                InstanceAspect.AroundEnter,

                Events.TestEventRemoveExit,
                InstanceAspect.AroundExit
            });
        }

        [TestMethod]
        public void AdviceAround_Instance_Static_Remove()
        {
            ExecStaticRemove();
            CheckSequence(new List<string> {
                InstanceAspect.AroundEnter,

                Events.TestStaticEventRemoveExit,
                InstanceAspect.AroundExit
            });
        }

        [TestMethod]
        public void AdviceAround_Instance_Methods()
        {
            ExecMethod();
            ExecIteratorMethod();
            ExecAsyncVoidMethod();
            ExecAsyncTaskMethod();
            ExecAsyncTypedTaskMethod();

            CheckSequence(new List<string> {
                InstanceAspect.AroundEnter,
                Events.TestMethodExit,
                InstanceAspect.AroundExit,

                //state machine executes after around
                InstanceAspect.AroundEnter,
                InstanceAspect.AroundExit,
                Events.TestIteratorMethodExit,

                InstanceAspect.AroundEnter,
                InstanceAspect.AroundExit,
                Events.TestAsyncMethodExit,

                InstanceAspect.AroundEnter,
                InstanceAspect.AroundExit,
                Events.TestAsyncMethodExit,

                InstanceAspect.AroundEnter,
                InstanceAspect.AroundExit,
                Events.TestAsyncMethodExit,
            });
        }

        [TestMethod]
        public void AdviceAround_Instance_Static_Methods()
        {
            ExecStaticMethod();
            ExecStaticIteratorMethod();
            ExecStaticAsyncVoidMethod();
            ExecStaticAsyncTaskMethod();
            ExecStaticAsyncTypedTaskMethod();

            CheckSequence(new List<string> {
                InstanceAspect.AroundEnter,
                Events.TestStaticMethodExit,
                InstanceAspect.AroundExit,

                //state machine executes after around
                InstanceAspect.AroundEnter,
                InstanceAspect.AroundExit,
                Events.TestStaticIteratorMethodExit,

                InstanceAspect.AroundEnter,
                InstanceAspect.AroundExit,
                Events.TestStaticAsyncMethodExit,

                InstanceAspect.AroundEnter,
                InstanceAspect.AroundExit,
                Events.TestStaticAsyncMethodExit,

                InstanceAspect.AroundEnter,
                InstanceAspect.AroundExit,
                Events.TestStaticAsyncMethodExit,
            });
        }
    }
}