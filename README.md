**Project Description**

The aim is to make a String Processor which allows a customizable pipeline.

The String Processor&#39;s main focus is speed efficiency and customizability. For that purpose each string will pass through 3 stages, each stage having a customizable number operations chosen from a subset.

You will have to implement an application which receives the path to a file containing strings, 1 on each line. The application will read all the strings in the file and then pass them to the String Processor which will run the strings through the 3 stages, and when finished write the output to a file.

**String Processor requirements**

- It will be 3 stageswhich will be run in sequence
- Any of following operations are possible to be added on each stage:
  - Lowercase – will convert all letters to lowercase
  - Uppercase – will convert all letters to uppercase
  - Sort – will sort letters within the line
  - Invert – will inverse the whole string line
  - Other operations can be added by the candidate
- Each stage should be easy configurable with the operations it will execute.
- In order for any string to move the next stage, the previous stage needs to be completed for that specific string - you cannot run a string directly through all the 3 stages
- At the end of the full processing cycle, when all the strings got through all the 3 stages, the String Processor notifies the caller that the processing is done

**Limitations**

- The amount of strings given will be less than 1000 and their length will be less than 1000
- The maximum amount of operations queued on a stage will be of 5. There is no restriction on the type of operations
- The operations from a stage will be executed in the order they were enqueued
- String Processor shall be delivered as a multi-threading solution

**String Processor API minimal requirements**

- A method will be provided to user to be able to enqueue / dequeue operations of a certain stage
- A method will be provided for the user to start processing
- A way to notify the user that the job is done will be provided

**Sample Input / Ouput**

**In.txt**

Assda drRRrfa PfffaASDv

EEll ggguur aaa lorEM ipsum dolor

Having the following configuration:

Stage1: lowercase

Stage2: remove spaces

Stage3: sort, Uppercase

**Out.txt**

AAAAADDDFFFFPRRRRSSSV

AAADEEEGGGILLLLMMOOOPRRSUUU

**Assignment Output**

- String Processor Object with public API to be used by other developers
- Project + source files containing code and internally used data structures
- Code to be compilable and usable in Visual Studio
- Inside the main, sample code of usage of the provided API
- You can use either C# or C++ to complete this task
