using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;

namespace Tube.Specs
{    
    [TaskName("fake task")]
    [TaskDependsOn("task1", "task2")]
    public class FakeTask : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            throw new System.NotImplementedException();
        }

        public void CallOnJobUpdated(FakeTaskContext context)
        {
            OnJobUpdated(context);
        }        
    }

    [TaskName("simple fake task")]
    public class SimpleFakeTask :Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            throw new NotImplementedException();
        }
    }

    [Subject(typeof(Task<FakeTaskContext>))]
    public class job_updated : WithSubject<FakeTask>
    {
        private static FakeTaskContext fakeTaskContext = new FakeTaskContext();
        private static JobUpdatedEventArgs<FakeTaskContext> eventArgs;
        Establish context = () => Subject.JobUpdated += (s, e) => eventArgs = e;
        Because of = () => Subject.CallOnJobUpdated(fakeTaskContext);
        It should_trigger_the_job_updated_event = () => eventArgs.Context.ShouldEqual(fakeTaskContext);
    }

    [Subject(typeof (Task<FakeTaskContext>))]
    public class get_name : WithSubject<FakeTask>
    {
        It should_have_the_name_defined_in_the_attribute = () => Subject.GetName().ShouldEqual("fake task");
    }

    [Subject(typeof(Task<FakeTaskContext>))]
    public class get_dependencies : WithSubject<FakeTask>
    {
        It should_have_the_dependencies_defined_in_the_attribute = () =>
            {
                Subject.GetDependencies().First().ShouldEqual("task1");
                Subject.GetDependencies().Last().ShouldEqual("task2");
            };
    }

    [Subject(typeof(Task<FakeTaskContext>))]
    public class get_dependencies_when_there_are_none : WithSubject<SimpleFakeTask>
    {
        It should_have_the_dependencies_defined_in_the_attribute = () => Subject.GetDependencies().Count().ShouldEqual(0);
    }
}
