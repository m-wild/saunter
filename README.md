# AsyncApi.Net

> If you have ever used this hell creation - Saunter, then you will understand why I want to rewrite it

Is an [AsyncAPI](https://github.com/asyncapi/asyncapi) documentation generator for dotnet.

ℹ Note that pre version 1.0.0, the API is regarded as unstable and **breaking changes may be introduced**.

This is a fork of the [Sauner library](https://github.com/m-wild/saunter/tree/main), which was rewritten for the sake of ease of use and minimizing the cost of implementation in the project.

## Roadmap

The current implementation has 3 goals.

* Ease of implementation in any project without limitation
* Providing an opportunity to describe any complex scheme
* Support for the current version `asyncapi`(with the possibility of updating to 3.0.0 after release)

### 1 priority

The main purpose of the stage works is to make it possible to describe an operation in 1 attribute without restrictions on the number of operations per method/class

* [X] To dotnet 7
* [X] To asyncapi 2.6.0
* [X] Set required and nullable props to schema
* [X] Give the opportunity to work with multiple operations in the one class/method
* [X] Kill channel attribute:

    ```csharp
    [SubscribeOperation("asw.tenant_service.tenants_history", OperationId = "TenantMessageConsumer", Summary = "Subscribe to domains events about tenants.", ChannelDescription = "Tenant events.")]
    public void PublishHelloWord(string content) { }
    ```

* [X] Rework message attribute:

    ```csharp
    [SubscribeOperation<BrokerHelloWorldDto>("asw.tenant_service.tenants_history", OperationId = "TenantMessageConsumer", Summary = "Subscribe to domains events about tenants.", ChannelDescription = "Tenant events.")]
    public void PublishHelloWord(string content) { }
    ```

    ```csharp
    [Message(Title = "Hello world, i`m class")]
    public record BrokerHelloWorldDto(string content);
    ```

* [X] Kill channel params attribute (auto detect parameters from channel name)

    ```csharp
    [SubscribeOperation<BrokerHelloWorldDto>("asw.tenant_service.{tenants_name}", OperationId = "TenantMessageConsumer")]
    public record BrokerHelloWorldDto(string content);
    ```

* [X] Redo the processing of multiple documents in the application (save default document with `null` name!!)

    ```csharp
    [SubscribeOperation<BrokerHelloWorldDto>("asw.tenant_service.{tenants_name}", OperationId = "TenantMessageConsumer", DocumentName = "Foo")]
    [SubscribeOperation<BrokerHelloWorldDto>("asw.tenant_service.{tenants_name}", OperationId = "TenantMessageConsumer")]
    public record BrokerHelloWorldDto(string content);
    ```

* [ ] Rewrite usage docs
* [ ] Nuget package
* [ ] Usability test on my environment
* [ ] Release !!

Known limitations of the version that will be received at this stage:

* There is no support for description and location for channel parameters from attributes (only from components ref)

![img](assets/1_priority.png)

### 2 priority

The main goal of the stage works is to expand the automatically generated part of the schema through xml-comments and improve the quality of the product

* [ ] Add generator output model validation
* [ ] Add xml-comments to output model
* [ ] Add `yaml` output document
* [ ] Rework and enrich unit tests
* [ ] Rework and enrich component tests with `TestHost`

### 3 priority

The main goal of this stage is to refine the remaining features of the async api (such as binding protocol) and develop a tool for describing detailed and complex schemes (without using attributes)

* [ ] Make a normal tool for describing a any complex asyncapi document (without attributes, static method on interface?....)
* [ ] Rework the binding protocols (now it's done terribly)

### 4 priority

The main goal of this stage is to automatically generate part of the scheme from native library objects for the protocols I use (`nats`, `signalR`)

* [ ] To dotnet 8
* [ ] Native work with `nats`
* [ ] Native work with `signalR`
* [ ] Native work with `swagger` (or wait asyncapi 3.0.0 ...?)

### Simple start

1. Install package from nuget - `TODO: add link`
2. Configure base generator params in `Program.cs`:

    ```csharp
       services.AddAsyncApiSchemaGeneration(o =>
        {
            o.AssemblyMarkerTypes = new[] { typeof(StreetlightsController) }; // add assemply marker
            o.AsyncApi = new AsyncApiDocument { Info = new Info { Title = "My application" }}; // introduce your application
        });
    ```

3. Map generator and ui in `Program.cs`:

    ```csharp
    app.MapAsyncApiDocuments();
    app.MapAsyncApiUi();
    ```

4. Set attributes to pub/sub methods:

    ```charp
    [PublishOperation<MyPayloadMessageType>("my_queue_name")]
    [PublishOperation<MyPayloadMessageType, MySecondPayloadMessageType>("my_queue_second_name")]
    public void MyMethod()
    ```

5. Run application and open endpoint - `/asyncapi/ui/`

## Getting Started (rewrite me pls)

See [StreetlightsAPI](https://github.com/yurvon-screamo/asyncapi.net/tree/main/examples/StreetlightsAPI) as example.

1. In the `ConfigureServices` method of `Startup.cs`, configure Saunter.

    ```csharp
    // Add generator to the application services. 
    services.AddAsyncApiSchemaGeneration(options =>
    {   
        // Specify example type(s) from assemblies to scan.
        options.AssemblyMarkerTypes = new[] {typeof(StreetlightMessageBus)};

        // Build as much (or as little) of the AsyncApi document as you like.
        // Saunter will generate Channels, Operations, Messages, etc, but you
        // may want to specify Info here.
        options.AsyncApi = new AsyncApiDocument
        {
            Info = new Info("Streetlights API", "1.0.0")
            {
                Description = "The Smartylighting Streetlights API allows you\nto remotely manage the city lights.",
                License = new License("Apache 2.0")
                {
                    Url = "https://www.apache.org/licenses/LICENSE-2.0"
                }
            },
            Servers =
            {
                { "mosquitto", new Server("test.mosquitto.org", "mqtt") }
            }
        };
    });
    ```

2. Add attributes to your classes which publish or subscribe to messages.

    ```csharp
    [AsyncApi] // Tells Saunter to scan this class.
    public class StreetlightMessageBus : IStreetlightMessageBus
    {
        [Channel("publish/light/measured")] // Creates a Channel
        [PublishOperation(typeof(LightMeasuredEvent), Summary = "Inform about environmental lighting conditions for a particular streetlight.")] // A simple Publish operation.
        public void PublishLightMeasuredEvent(Streetlight streetlight, int lumens) {}
    ```

3. Add saunter middleware to host the AsyncApi json document. In the `Configure` method of `Startup.cs`:

    ```csharp
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapAsyncApiDocuments();
        endpoints.MapAsyncApiUi();
    });
    ```

4. Use the published AsyncApi document:

    ```jsonc
    // HTTP GET /asyncapi/asyncapi.json
    {
        // Properties from Startup.cs
        "asyncapi": "2.1.0",
        "info": {
            "title": "Streetlights API",
            "version": "1.0.0",
            "description": "The Smartylighting Streetlights API allows you\nto remotely manage the city lights.",
           // ...
        },
        // Properties generated from Attributes
        "channels": {
            "light/measured": {
            "publish": {
                "operationId": "PublishLightMeasuredEvent",
                "summary": "Inform about environmental lighting conditions for a particular streetlight.",
            //...
    }
    ```

5. Use the published AsyncAPI UI:

    ![AsyncAPI UI](https://raw.githubusercontent.com/tehmantra/saunter/main/assets/asyncapi-ui-screenshot.png)

## Configuration

See [the options source code](https://github.com/tehmantra/saunter/blob/main/src/Saunter/AsyncApiOptions.cs) for detailed info.

Common options are below:

```c#
services.AddAsyncApiSchemaGeneration(options =>
{
    options.AssemblyMarkerTypes = new[] { typeof(Startup) };   // Tell Saunter where to scan for your classes.
    
    options.AddChannelItemFilter<MyChannelItemFilter>();       // Dynamically update ChanelItems
    options.AddOperationFilter<MyOperationFilter>();           // Dynamically update Operations
    
    options.Middleware.Route = "/asyncapi/asyncapi.json";      // AsyncAPI JSON document URL
    options.Middleware.UiBaseRoute = "/asyncapi/ui/";          // AsyncAPI UI URL
    options.Middleware.UiTitle = "My AsyncAPI Documentation";  // AsyncAPI UI page title
}
```

## JSON Schema Settings

The JSON schema generation can be customized using the `options.JsonSchemaGeneratorSettings`. Saunter defaults to the popular `camelCase` naming strategy for both properties and types.

For example, setting to use PascalCase:

```c#
services.AddAsyncApiSchemaGeneration(options =>
{
    options.JsonSchemaGeneratorSettings.TypeNameGenerator = new DefaultTypeNameGenerator();

    // Note: need to assign a new JsonSerializerSettings, not just set the properties within it.
    options.JsonSchemaGeneratorSettings.SerializerSettings = new JsonSerializerSettings 
    {
        ContractResolver = new DefaultContractResolver(),
        Formatting = Formatting.Indented;
    };
}
```

You have access to the full range of both [NJsonSchema](https://github.com/RicoSuter/NJsonSchema) and [JSON.NET](https://github.com/JamesNK/Newtonsoft.Json) settings to configure the JSON schema generation, including custom ContractResolvers.

## Bindings

Bindings are used to describe protocol specific information. These can be added to the AsyncAPI document and then applied to different components by setting the `BindingsRef` property in the relevant attributes `[OperationAttribute]`, `[MessageAttribute]`, `[ChannelAttribute]`

```csharp
// Startup.cs
services.AddAsyncApiSchemaGeneration(options =>
{
    options.AsyncApi = new AsyncApiDocument
    {
        Components = 
        {
            ChannelBindings = 
            {
                ["my-amqp-binding"] = new ChannelBindings
                {
                    Amqp = new AmqpChannelBinding
                    {
                        Is = AmqpChannelBindingIs.RoutingKey,
                        Exchange = new AmqpChannelBindingExchange
                        {
                            Name = "example-exchange",
                            VirtualHost = "/development"
                        }
                    }
                }
            }
        }
    }
});
```

```csharp
[Channel("light.measured", BindingsRef = "my-amqp-binding")] // Set the BindingsRef property
public void PublishLightMeasuredEvent(Streetlight streetlight, int lumens) {}
```

Available bindings:

* [AMQP](https://github.com/tehmantra/saunter/tree/main/src/Saunter/AsyncApiSchema/v2/Bindings/Amqp)
* [HTTP](https://github.com/tehmantra/saunter/tree/main/src/Saunter/AsyncApiSchema/v2/Bindings/Http)
* [Kafka](https://github.com/tehmantra/saunter/tree/main/src/Saunter/AsyncApiSchema/v2/Bindings/Kafka)
* [MQTT](https://github.com/tehmantra/saunter/tree/main/src/Saunter/AsyncApiSchema/v2/Bindings/Mqtt)

## Multiple AsyncAPI documents

You can generate multiple AsyncAPI documents by using the `ConfigureNamedAsyncApi` extension method.

```cs
// Startup.cs

// Add Saunter to the application services. 
services.AddAsyncApiSchemaGeneration(options =>
{
    // Specify example type(s) from assemblies to scan.
    options.AssemblyMarkerTypes = new[] {typeof(FooMessageBus)};
}

// Configure one or more named AsyncAPI documents
services.ConfigureNamedAsyncApi("Foo", asyncApi => 
{
    asyncApi.Info = new Info("Foo API", "1.0.0");
    // ...
});

services.ConfigureNamedAsyncApi("Bar", asyncApi => 
{
    asyncApi.Info = new Info("Bar API", "1.0.0");
    // ...
});
```

Classes need to be decorated with the `AsyncApiAttribute` specifying the name of the AsyncAPI document.

```cs
[AsyncApi("Foo")]
public class FooMessageBus 
{
    // Any channels defined in this class will be added to the "Foo" document
}


[AsyncApi("Bar")]
public class BarMessageBus 
{
    // Any channels defined in this class will be added to the "Bar" document
}
```

Each document can be accessed by specifying the name in the URL

```json
// GET /asyncapi/foo/asyncapi.json
{
    "info": {
        "title": "Foo API"
    }
}

// GET /asyncapi/bar/asyncapi.json
{
    "info": {
        "title": "Bar API"
    }
}
```

## Contributing

See our [contributing guide](https://github.com/tehmantra/saunter/blob/main/CONTRIBUTING.md/CONTRIBUTING.md).

Feel free to get involved in the project by opening issues, or submitting pull requests.

You can also find me on the [AsyncAPI community slack](https://asyncapi.com/slack-invite).

## Thanks

* This project is heavily inspired by [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore).
* We use [NJsonSchema](https://github.com/RicoSuter/NJsonSchema) for the JSON schema heavy lifting,
