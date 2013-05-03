using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;

namespace Tube.IntegrationTests
{
    public class CakeMaker
    {
        public void WeighIngredients()
        {
            Weighed = true;
        }

        public bool Weighed { get; set; }

        public void MixIngredients()
        {
            Mixed = true;
        }

        public bool Mixed { get; set; }

        public void Bake()
        {
            Baked = true;
        }

        public bool Baked { get; set; }

        public void PrepareIcing()
        {
            IcingPrepared = true;
        }

        public bool IcingPrepared { get; set; }

        public void Decorate()
        {
            Decorated = true;
        }

        public bool Decorated { get; set; }
    }
    [TaskName("weigh ingredients")]
    public class Weigher : Task<CakeMaker>
    {
        public List<string> Log = new List<string>();
        public override void Execute(CakeMaker context)
        {
            context.WeighIngredients();
        }
    }

    [TaskName("mix ingredients")]
    [TaskDependsOn("weigh ingredients")]
    public class Mixer : Task<CakeMaker>
    {
        public override void Execute(CakeMaker context)
        {
            context.MixIngredients();
        }
    }

    [TaskName("bake")]
    [TaskDependsOn("mix ingredients")]
    public class Baker : Task<CakeMaker>
    {
        public override void Execute(CakeMaker context)
        {
            context.Bake();
        }
    }

    [TaskName("prepare icing")]
    [TaskDependsOn("mix ingredients")]
    public class IcingPreparer : Task<CakeMaker>
    {
        public override void Execute(CakeMaker context)
        {
            context.PrepareIcing();
        }
    }

    [TaskName("decorate")]
    [TaskDependsOn("prepare icing", "bake")]
    public class CakeDecorator : Task<CakeMaker>
    {
        public override void Execute(CakeMaker context)
        {
            context.Decorate();
        }
    }

    [TaskName("make cake")]
    [TaskDependsOn("decorate", "bake")]
    public class CakeBuilder : Task<CakeMaker>
    {
        public override void Execute(CakeMaker context)
        {

        }
    }


    public class SamplePipeline
    {
        private static IPipeline<CakeMaker> pipeline;
        private static CakeMaker cakeMaker = new CakeMaker();
        Establish context = () =>
            pipeline = PipelineFactory.Create<CakeMaker>()
                                      .RegisterTask(new Weigher())
                                      .RegisterTask(new Mixer())
                                      .RegisterTask(new Baker())
                                      .RegisterTask(new IcingPreparer())
                                      .RegisterTask(new CakeDecorator())
                                      .RegisterTask(new CakeBuilder());
        Because of = () => pipeline.Run("make cake", cakeMaker);
        private It should_have_been_weighed = () => cakeMaker.Weighed.ShouldBeTrue();
        It should_have_been_mixed = () => cakeMaker.Mixed.ShouldBeTrue();
        It should_have_been_baked = () => cakeMaker.Baked.ShouldBeTrue();
        It should_have_had_icing_prepared = () => cakeMaker.IcingPrepared.ShouldBeTrue();
        It should_have_been_decorated = () => cakeMaker.Decorated.ShouldBeTrue();

    }
}
