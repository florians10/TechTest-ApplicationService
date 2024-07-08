# Log

## Asummptions

Products should be easily added and so new administrators. The code should be easy to modify in both of these circumstances.
Adapter pattern can be used for switching between administrators 
Strategy pattern can be used for choosing the intended administrator for different product types - this can be done using a registry or dependency injection
The validation of current pallete of products is really similar and most of it can be shared, where specific variations can come from static configuration (weather file / database / etc)

## Decisions

Decided to approach an adapter pattern to simplify the usage of various third parties into a single internal interface.
Applications for different products can then be routed to the configured selected administrator (using a strategy pattern which in our case is using a simple registry class)
Dependency injection to be used to help out with unit testing

## Observations

Administrators use sync/async calls and hence for a better operability, the internal adapter interface can then be async to be compatible and allow async execution.

## Todo

The implementation of each adapter might need to be revised based on intended logic:
 * what are the actions in case of various reponse types
 * how the actions be dependent on eachother when multiple calls are needed against an administrator
 * tests for adapters have not been considered - but ccould be easily done in a similar fashion as the main ApplicationProcesorTests
 ** To validate the desired execution flow