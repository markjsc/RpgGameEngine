# Notes

Thank you for the opportunity to provide an example of my work. I've contributed to custom developed
systems for a wide range of industries before, but I think this is my first experience with game
development. (Although I was an avid PC and console gamer for many years - I don't think that counts.)

## Assumptions
I made the following assumptions in cases where I wasn't quite clear on the details.

* For simplicity of this example, I assumed the Value would be integer (throwing an exception if it's not).
  Additional features could be added to support varying types. (For example, the Equality comparison operators
  could still be performed for non-integer values. But type checking would need to be applied in order to
  apply the math operators.)
* Xml Structure - I assume there will be some additional XML wrapping before the array of Element elements
  (i.e.
```xml
    <SomeRoot>
        <SomeContainer>
            <Element>…</Element>
        </SomeContainer>
    </SomeRoot>
```
## Comments:
Some additional comments for clarification:

* I realize there are some optimizations that will need to be performed to make this code more device-friendly
  (i.e. reducing iterations by using more efficient types of storage). The iterations are all about expediency.
* I used quite a bit of defensive logic (lots of try/catches and repetitive checks for items may not be found
  when expected). I realize at least some of this may not be necessary in paradigms where the inputs and 
  outpus are more tightly controlled (i.e. a Game Designer defining  the rules at design time, but no 
  modifications at runtime). This is mostly out of habit based on my experience in paradigms with 
  constantly-shifting assumptions (i.e. concurrent operations, unreliable data, etc).
* I've become accustomed to relying on ReSharper and Visual Studio Enterprise to help me get by with lazy 
  keyboard habits and the like. I don't have the same toolset on my personal PC (where I did the interview 
  development). So this is probably a bit more sloppy and less professional than my normal output, at 
  least as long as the toolset crutch exists.