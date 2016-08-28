# C# Test
Hitcents C# Programming Test

## Rules

Please don't distribute this test or post it on a public Github repository.

Use any resources online you like and any third party libraries you like. It should be easy for me to check out and build your code (maybe just open a Visual Studio solution and hit play), include a build script or instructions if needed.

Write your unit tests in a way in which they are easily runnable, MsTest/NUnit are easily ran from VS without special extensions. Include instructions if you prefer another unit test library.

Include a "front-end" or UI to control your work. It can be a simple console application with text options, a WPF app, an ASP.Net website, a mobile Xamarin app. Do what you feel comfortable with.

## Task

You are tasked to implement a XML-based markup language for RPG games. It is simple to understand and meant to be written by game designers to express the logic behind the game.

The XML language contains these types of XML elements:
* Element - an arbitrary "game object" that holds meaning within the game
  * Id - unique identifier
  * Value - Holds a number (or maybe string) for the value of this object
  * List of `<Element>` - for child elements, can be infinitely deep
* Action - an action you perform to modify the game's state
  * Id - unique identifier
  * List of `<Setter>` - operations that run on `<Element>` objects
* Trigger - something that happens as the result of a value changing
  * Target - the element to watch
  * Comparison - boolean operators: >, <, ==, !=, <=, >=
  * List of `<Setter>` - operations that run on `<Element>` objects
* Setter - an operation that modifies a value
  * Target - the element to modify
  * Operation - math operators, or assignment: =, +, -, /, *
  * Value - the value to use for the operation

For example, a game hierarchy might look something like:
```xml
<Element Id="CoolGame">
  <Element Id="Player">
    <Element Id="Level" Value="1" />
    <Element Id="XP" Value="0" />
    <Element Id="HP" Value="50" />
    <Action Id="GainXP">
      <Setter Target="XP" Operation="+" Value="10" />
    </Action>
    <Trigger Target="XP" Comparison=">=" Value="10">
      <Setter Target="Level" Operation="=" Value="2" />
    </Trigger>
  </Element>
</Element>
```

What this does, is create an object tree:
* CoolGame
 * Player
  * Level 1
  * XP 0
  * HP 50
  * [Action] GainXP
  * [Trigger] watches XP

When you run `GainXP`, `XP` should go to 10, the trigger should run and `Level` should go to 2. You can think of this thing as a state machine that *could* be used to drive the logic behind any RPG game you can think of.

## Deliverables

* Track how long you worked on this: 4 hours, 8 hours, etc.
* A C# library for loading XML and running game logic, should be a nice interface that other programmers can reuse.
* A unit test suite.
* A "front-end" to control your library (don't worry it can be really ugly)
 * Should have the ability for me to somehow insert my own XML file to play around.
