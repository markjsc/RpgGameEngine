 Notes

Thank you for the opportunity to provide an example of my work. I've contributed to custom developed
systems for a wide range of industries before, but I think this is my first experience with game
development. (Although I was an avid PC and console gamer for many years - I don't think that counts.)

## Assumptions
I made the following assumptions in cases where I wasn't quite clear on the details.

* For simplicity of this example, I assumed the Value would be integer (throwing an exception if it's not).
  Additional features could be added to support varying types. (For example, the Equality comparison operators
  could still be performed for non-integer values. But type checking would need to be applied in order to
  apply the math operators.)
* Separate Classes for Raw XML vs. Domain Logic - I created seprate model classes for the XML deserializing and
  the Domain Logic. Even in a small, quick effort like this, it's helpful to have the logic separated. The
  result is that the domain classes don't have to be polluted with the XML serialization attributes and
  they can even have different property types, if needed.
* Xml Structure - I originally implemented XML Deserialization with the expectation that the incoming XML
  would be required to have some outer tags that are required for properly deserializable XML. But I ended up
  adding a hack in the XML Loader to stuff the incoming XML (literally in the structure provided in the original 
  ReadMe.MD) inside the expected tags. I was unclear on whether the supported XML should be literally what
  was provided, or if there's some flexibility there.
  The result of the hack is:
```xml
    <SomeRoot>
        <SomeContainer>
            <!--XML provided to the Loader-->
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