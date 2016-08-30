 # Mark's Notes

Thank you for the opportunity to provide an example of my work. I've contributed to custom developed
systems for a wide range of industries before, but I think this is my first experience with game
development. (Although I was an avid PC and console gamer for many years - I don't think that counts.)
What an enjoyable and rewarding effort this has been!

I wish I had more time to implement features that I generally use on larger systems - logging, better
validation and error handling, prettier styling, etc. I attempted to leave TODO comments in appropriate
places indicating what I would do given a bit more time.

## Time Spent
I spent just north of **17 hours** on this project. (In full disclosure, some of the time spent on Saturday
and Sunday were not as focused as my usual work-day efforts due to family commitments.)

## Project Instructions
To run the Desktop Client (WPF):
1. Open Hitcents.Interview.sln with Visual Studio (I used Visual Studio 2015 Community Edition)
2. Set Hitcents.Interview.AwesomeRpg.DesktopClient as the startup project
3. Press F5 (or Debug -> Start Debugging)

To run the Unit Tests:
1. Open Hitcents.Interview.sln with Visual Studio
2. Open the Test Explorer (Test -> Windows -> Test Explorer)
3. Click the link to Run All tests

## Project Structure
I used the name "AwesomeRpg" because I'm not great at creatively naming systems. :-(

The reusable library projects are all built as Portable libraries, using the default targets 
(.NET 4.5, ASP.NET Core 1.0, Windows 8, and Windows Phone 8.1). This can obviously be changed 
as needed. The Desktop Client project, as mentioned above, is WPF and the Unit Tests project is
a standard .NET Unit Test project.

* __Contracts__ (Hitcents.Interview.AwesomeRpg.Contracts) - interfaces that can be implemented by
  the current proejcts or any future ones; also data and domain models. There is no logic here.
* __Desktop Client__ (Hitcents.Interview.AwesomeRpg.DesktopClient) - a typical WPF client project.
  I did not leverage any frameworks (like Prism or another MVVM framework) because of the small
  size of this project. That would be a logical next step if the client needs to do more. There
  is almost no logic here, therefore the unit tests do not exercise any of this code. You can
  load your own XML from a file, paste from the clipboard, or insert a pre-defined sample.
  (_Please forgive the hideous UI styling. Your instructions said 'Ugly' which just happens to 
  describe my standard UX design approach!_)
* __Engine__ (Hitcents.Interview.AwesomeRpg.Engine) - the main business logic for the system. The
  GameContext class holds the Game State, facilitates running actions and firing triggers. As you
  will see from some of the comments in the code, this could definitely stand to be optimized a bit
  more, depending on performance needs. I was focused on core functionality and the logical relationships
  of the entities and did not spend as much time looking for optimization opportunities. The 
  GameStateNavigator class performs the basic traversing through the Game State tree to locate Elements,
  Actions and Triggers. These classes are heavily tested by the Unit Tests.
* __Loaders__ (Hitcents.Interview.AwesomeRpg.Loaders) - this handles deserializing of XML and converting the
  data into domain objects for consumption by GameContext. Any other loaders (besides XML) could be
  used to feed data into the GameStateLoader. There are some opportunities to improve GameStateLoader
  by adding more validation - I just didn't have enough time to complete that.
* __Unit Tests__ (Hitcents.Interview.AwesomeRpg.Tests) - Your average, everyday Unit Test project. Some
  of the tests are very coarse (i.e. XML Loader Tests), while others attempt to exercise every possible
  scenario (i.e. Game Context Tests). I didn't have my usual test coverage tools available, so I'm sure
  there are some test coverage gaps - hopefully I was able to validate the key functionality.
  

## Assumptions
I made the following assumptions in cases where I wasn't quite clear on the details.

* __Strings vs. Numerics__ - Based on a comment in the provided Readme.MD, I allowed the Values to be strings or 
  numerics. However the only supported Operation for strings is _Assign_. Also, the only supported Comparisons 
  for strings are _Equal_ and _NotEqual_. 
  **One critical note:** If an alpha is provided when a numeric Operation or Comparison is to be
  performed then the value will be replaced with a zero. This is probably not desired and a better approach
  would be to validate the configuration and throw an exception when alphas are provided where numerics are
  expected.
* __Case Insensitivity__ I made the assumption that the Element and Action Ids could be referenced without
  regard to case sensitivity. This can be changed in ONE place (GameStateNavigator) if case sensitive Ids 
  are preferred.
* __Separate Classes for Raw XML vs. Domain Logic__ - I created seprate model classes for the XML deserializing and
  the Domain Logic. Even in a small, quick effort like this, it's helpful to have the logic separated. The
  result is that the domain classes don't have to be polluted with the XML serialization attributes and
  they can even have different property types or structure, if needed.
* __Xml Structure__ - I originally implemented XML Deserialization with the expectation that the incoming XML
  would be required to have some outer tags that are required for properly deserializable XML. But I ended up
  adding a hack in the XML Loader to stuff the incoming XML (literally in the structure provided in the original 
  ReadMe.MD) inside the expected tags. I was unclear on whether the supported XML should be literally what
  was provided, or if there's some flexibility there.
  The result of the hack is:
```xml
    <GameConfig>
        <Elements>
            <!--XML provided to the Loader-->
            <Element>…</Element>
        </Elements>
    </GameConfig>
```
## Comments
Some additional comments for clarification:

* I realize there are some optimizations that will need to be performed to make this code more device-friendly
  (i.e. reducing iterations by using more efficient types of storage). The iterations are all about accuracy and expediency.
* I used quite a bit of defensive logic (lots of try/catches and repetitive checks for items may not be found
  when expected). I realize at least some of this may not be necessary in paradigms where the inputs and 
  outpus are more tightly controlled (i.e. a Game Designer defining  the rules at design time, but no 
  modifications at runtime). This is mostly out of habit based on my experience in paradigms with 
  constantly-shifting assumptions (i.e. concurrent operations, unreliable data, etc).
* I've become accustomed to relying on ReSharper and Visual Studio Enterprise to help me get by with lazy 
  keyboard habits, unit test coverage verification and the like. I don't have the same toolset on my 
  personal PC (where I did the interview  development) - I only have Visual Studio 2015 Community Edition.
  So this is probably a bit more sloppy and less professional than my normal output, at least until I break
  my addiction to tooling!
* I spent years as a heavy code commenter until I read "Clean Code" (by Robert "Uncle Bob" Martin). I've changed
  my style so that the code is more self-documenting and try to keep comments to a minimum due to the maintenance
  liability of having comments not reflect the code over many touches in a system's lifespan. However, I'm
  happy to adapt to any style that's requested

## Final Thoughts
Even if this results in a clear 'No' on hiring me for your current open position, I would appreciate any
feedback you can offer to help me continue growing professionally. I would also appreciate being considered
for any future positions at Hitcents that you think may be a better fit.

Thanks again for the opportunity!

**Mark Johnston**  
Mark.Johnston@yahoo.com  
270-903-7276