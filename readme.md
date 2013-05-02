#Tube

Tube is a basic task pipeline for C#

Here's how it works
===================

First, define your tasks:

```
[TaskName("weigh ingredients")]
public class Weigher : Task<CakeMaker>
{
  public override void Execute(CakeMaker context)
  {
    context.WeighIngredients();
    Console.WriteLine("Ingredients weighed");
  } 
}

[TaskName("mix ingredients")]
[TaskDependsOn("weigh ingredients")]
public class Mixer : Task<CakeMaker>
{
  public override void Execute(CakeMaker context)
  {
    context.MixIngredients();
    Console.WriteLine("Ingredients mixed");
  } 
}

[TaskName("bake")]
[TaskDependsOn("mix ingredients")]
public class Baker : Task<CakeMaker>
{
  public override void Execute(CakeMaker context)
  {
    context.Bake();
    Console.WriteLine("Cake baked");
  } 
}

[TaskName("prepare icing")]
[TaskDependsOn("mix ingredients")]
public class IcingPreparer : Task<CakeMaker>
{
  public override void Execute(CakeMaker context)
  {
    context.PrepareIcing();
    Console.WriteLine("Cake iced");
  } 
}

[TaskName("decorate")]
[TaskDependsOn("prepare icing", "bake")]
public class CakeDecorator : Task<CakeMaker>
{
  public override void Execute(CakeMaker context)
  {
    context.Decorate();
    Console.WriteLine("Cake decorated");
  } 
}

[TaskName("make cake")]
[TaskDependsOn("decorate", "bake")]
public class CakeBuilder : Task<CakeMaker>
{
  public override void Execute(CakeMaker context)
  {
    Console.WriteLine("Cake made");
  } 
}

```
Create a pipeline and configure that all (autoconfigure coming very soon):
```
var pipeline = PipelineFactory.Create<CakeMaker>()
                              .RegisterTask(new Weigher())
                              .RegisterTask(new Mixer())
                              .RegisterTask(new Baker())
                              .RegisterTask(new IcingPreparer())
                              .RegisterTask(new CakeDecorator())
                              .RegisterTask(new CakeBuilder());
```
Git Er Done
===========
```
var cakeMaker = new CakeMaker();
pipeline.Run("make cake", cakeMaker);
```
Output:
```
Ingredients weighed
Ingredients mixed
Cake baked
Cake iced
Cake decorated
Cake made
```
