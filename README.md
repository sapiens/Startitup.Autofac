# Strapboot.Autofac

Integration of [Startitup](https://github.com/sapiens/Startitup) with Autofac

## License

Apache 2.0

## Usage

It attaches extensions method to `StartupContext` so, your config object should inherit from StartupContext.

```csharp

public class AppSettings:StartupContext
{
}

public class ConfigTask_2_Something
{
  public void Run(AppSetings cfg)
  {
   //do stuff
   
   //register things into container
  cfg.ConfigureContainer(cb=> /* autofac config for this task */);
    
    //use container
  cfg.Container().Resolve<MyService>();
  }
  
}

```
