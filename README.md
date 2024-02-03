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
Example:
```
var primeNumbers = new SetParameter<int>("PrimeNumber", new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 });
```

### RangePatameter
It generates parameters base on ranges. It is usually numeric value.

### JoinParameters
It is not real parameter, it just joining more parameters together. For example when you have one RangeParameter form 5 to 20 and second is SetParameter with values 3, 50, 81. After join it will be first applied RangeParameter and then SetParameter.

## Groups
Groups are useful when you combinate parameters. It is similar to put digit wheels to one line.

### CartesianParameterGroup
This group combine every parameter with each other (similar like mechanical counter). When first parameter use all defined values in iterations, then second parameter is set on next value and first parameter is set to first value.

### IndependentParameterGroup
In this group every counter is incremented until use all values. When all values are used, it appears reset and it starts use values from begining. It iterate until all values from longer set of values of parameter are not used.

## Output
Creating output has two options.

### ArgumentGenerator
This generate TasetCaseData object with anonymous parameters as is described in NUnit documentation.

### ObjectGenerator
It generates predefined objects
