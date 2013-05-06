#Tube

Tube is a basic task pipeline for C#

```
install-package tube
```

Create some tasks
-

```c#
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
Create a pipeline and register tasks (autoconfigure coming very soon):
-
```c#
var factory = new PipelineFactory();
var pipeline = factory.Create<CakeMaker>()
                      .RegisterTask<Weigher>()
                      .RegisterTask<Mixer>()
                      .RegisterTask<Baker>()
                      .RegisterTask<IcingPreparer>()
                      .RegisterTask<CakeDecorator>()
                      .RegisterTask<CakeBuilder>();
```
Run the task
-
```c#
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
Messaging
-
Subscribe to update messages for your task like this:
```c#
pipeline.Subscribe<TaskUpdated>(x => Console.WriteLine(x.Message))
```
Publish messages from your tasks like this:
```c#
[TaskName("foobar")]
public class CakeBuilder : Task<CakeMaker>
{
  public override void Execute(CakeMaker context)
  {
    //some codings
    PublishMessage(new TaskUpdated{Message = "It's all going fine"));    
  } 
}
```
Config
-
You probably want to use your own DI container. Fortunately this is easy, you just need to create your own instance resolver:

```c#
public class MyInstanceResolver : IInstanceResolver
{
  public object Create(Type type)
  {
    //your container resolve code
  }
}
```
And tell your PipelineFactory to use it:
```c#
var pipelineFactory = new PipelineFactory();
pipelineFactory.Configure().SetInstanceResolver(new MyInstanceResolver());
```
