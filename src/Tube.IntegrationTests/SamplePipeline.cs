using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;

namespace Tube.IntegrationTests
{
    public class ProcessingJob
    {
        public ProcessingJob()
        {
            Log = "";
        }

        public bool Initialized { get; set; }

        public bool Munged { get; set; }

        public bool Perfected { get; set; }

        public string Log { get; set; }

        public bool Polished { get; set; }
    }

    [TaskName("initialize")]
    public class Initializer : Task<ProcessingJob>
    {
        public override void Execute(ProcessingJob context)
        {
            context.Initialized = true;
            context.Log += "initialized";
            OnJobUpdated(context);
        }
    }

    [TaskName("munge")]
    [TaskDependsOn("perfect")]
    public class Munger : Task<ProcessingJob>
    {
        public override void Execute(ProcessingJob context)
        {
            context.Munged = true;
            context.Log += "_munged";
            OnJobUpdated(context);
        }
    }

    [TaskName("perfect")]
    [TaskDependsOn("initialize")]
    public class Perfector : Task<ProcessingJob>
    {
        public override void Execute(ProcessingJob context)
        {
            context.Perfected = true;
            context.Log += "_perfected";
            OnJobUpdated(context);
        }
    }

    [TaskName("polish")]
    [TaskDependsOn("munge")]
    public class Polisher : Task<ProcessingJob>
    {
        public override void Execute(ProcessingJob context)
        {
            context.Polished = true;
            context.Log += "_polished";
            OnJobUpdated(context);
        }
    }

    public class SamplePipeline
    {
        private static IPipeline<ProcessingJob> pipeline;
        private static ProcessingJob job = new ProcessingJob();
        Establish context = () => 
            pipeline = PipelineFactory.Create<ProcessingJob>()
                                      .RegisterTask(new Polisher())
                                      .RegisterTask(new Initializer())
                                      .RegisterTask(new Munger())
                                      .RegisterTask(new Perfector());
        Because of = () => pipeline.Run("polish", job);
        It should_have_been_initialized = () => job.Initialized.ShouldBeTrue();
        It should_have_been_munged = () => job.Munged.ShouldBeTrue();
        It should_have_been_perfected = () => job.Perfected.ShouldBeTrue();
        It should_have_been_polished = () => job.Polished.ShouldBeTrue();
        It should_have_outputted_in_the_correct_order = () => job.Log.ShouldEqual("initialized_perfected_munged_polished");
    }
}
