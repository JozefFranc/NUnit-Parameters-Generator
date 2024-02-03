# NUnit-Parameters-Generator
Generate parameters for NUnit tests

## Intro
When you need create better unit tests or just want tests all possibilities, usually you finish with parametrized tests. Sometimes creating TestCase-s is more complicated and you found NUnit generators. It has great possibilities, but creating generator which generate all possible parameters combination isn't easy at all.

This is exactly the point where you need avoid complexity of generators and simply create generator. Intention of this library is simplifing creation of test generators.

## Concept
Testing all combination of parameters is one possible way. But how to generate all possible combination?

Do you what is mechanical counter?

![Mechanical counter](docs/mechanical-counter.jpg)

When you don't know here is explanation	[How mechanical counters work](https://youtu.be/rjWfIiaOFR4?feature=shared)

If you imagine variables as one of this digit wheel, then you found principle of generating all combinations. Rest is easy.

You need create line of digit wheels, each for one parameter. Now when you rotate first digit wheel it will rotate next digit wheel.

## Parameters
It works like digit wheels. It use all predefined values when you iterate over it.

### SetParamer
It allow us genrates parameters based on predefined set.

**Example:**
```
var primeNumbers = new SetParameter<int>("PrimeNumber", new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 });
```
Where:<br/>
int - type of parameter<br/>
"PrimeNumber" - name of parameter (will be used for output)<br/>
array of parameters

### RangePatameter
It generates parameters base on ranges. It is usually numeric value.

**Example:**
```
var temperatureRange = new RangeParameter<int>("Temperature", 20, 40);
var weightRange = new RangeParameter<decimal>("Weight", 10.0m, 20.0m, 0.01m);
```
Where:<br/>
int, decimal - type of parameter<br/>
"Temperature", "Weight" - name of parameter (will be used for output)<br/>
20, 10.0m - start of range
40, 20.0m - end of range
0.01m - step in range (this needs to be defined for ranges with floating points)

### JoinParameters
It is not real parameter, it just joining more parameters together. For example when you have one RangeParameter form 5 to 20 and second is SetParameter with values 3, 50, 81. After join it will be first applied RangeParameter and then SetParameter.

**Example:**
```
var specificWeights = new SetParameter<decimal>("SpecificWeights", new[] { 10.0m, 20.0m });
var weightRange1 = new RangeParameter<decimal>("WeightRange1", 0m, 1m, 0.1m);
var weightRange2 = new RangeParameter<decimal>("WeightRange2", 2m, 3m, 0.1m);
var weights = new JoinParameters("Weight", weightRange1, weightRange2, specificWeights);
```
Where:<br/>"Weight" - name of parameter (will be used for output)<br/>
array of joined parameters

## Groups
Groups are useful when you combinate parameters. It is similar to put digit wheels to one line. Groups can be nested.

### CartesianParameterGroup
This group combine every parameter with each other (similar like mechanical counter). When first parameter use all defined values in iterations, then second parameter is set on next value and first parameter is set to first value.

**Example:**
```
var cartesianGroup1 = new CartesianParameterGroup(primeNumbers, temperatureRange);
var cartesianGroup2 = new CartesianParameterGroup(cartesianGroup1, weightRange);
```
Where:<br/>
array of parameters (in second case group and parameter)

### IndependentParameterGroup
In this group every counter is incremented until use all values. When all values are used, it appears reset and it starts use values from begining. It iterate until all values from longer set of values of parameter are not used.

**Example:**
```
var indenpandentGroup1 = new IndependentParameterGroup(primeNumbers, temperatureRange);
var indenpandentGroup2 = new IndependentParameterGroup(indenpandentGroup1, cartesianGroup2);
```
Where:<br/>
array of parameters (in second case groups)

## Output
Creating output has two options.

### ArgumentGenerator
This generate TestCaseData object with anonymous parameters as is described in NUnit documentation.

**Example:**
```
var generator = new ArgumentGenerator(indenpandentGroup2);
foreach (TestCaseData parameter in generator)
{
    Console.WriteLine(parameter.Arguments[0].ToString());
}
```
Where:<br/>
indenpandentGroup2 - top level group for iterations

### ObjectGenerator
It generates predefined objects

**Example:**
```
class ScaleTestData
{
    public decimal Weight { get; set; }
}

static void Example()
{
    var weightRange = new RangeParameter<decimal>("Weight", 10.0m, 20.0m, 0.01m);

    var indenpandentGroup1 = new IndependentParameterGroup(weightRange);

    var generator = new ObjectGenerator<ScaleTestData>(indenpandentGroup1);
    foreach (ScaleTestData parameter in generator)
    {
        Console.WriteLine(parameter.Weight.ToString());
    }
}
```
Where:<br/>
ScaleTestData - class which will be used for output. Property name and parameter name must be the same.
indenpandentGroup1 - top level group for iterations. Generators accepting parameter directly as well.
