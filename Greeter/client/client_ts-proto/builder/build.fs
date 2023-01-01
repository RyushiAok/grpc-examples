open Fake.Core
open Fake.IO 
open Fake.Core.TargetOperators
open Fake.JavaScript

let init () =  
    Target.create "Clean" (fun _ ->
        printfn "clean"  
        Shell.cd __SOURCE_DIRECTORY__ |> ignore
        Shell.cd ".." |> ignore  
    ) 

    Target.create "CodeGen" (fun _ ->
        Yarn.exec("codegen") id |> ignore
    )

    Target.create "Dev" (fun _ ->  
        Yarn.exec("codegen") id |> ignore
        task { Yarn.exec("dev") id |> ignore} |> ignore
        ()
    ) 
    
    Target.create "Build" (fun _ ->  
        Yarn.exec("codegen") id |> ignore
        Yarn.exec("build") id |> ignore 
        ()
    ) 

    "Clean" ==> "CodeGen" ==> "Build" |> ignore
    "Clean" ==> "CodeGen" ==> "Dev" |> ignore


[<EntryPoint>]
let main args =  
    []//args |> Seq.toList
    |> Context.FakeExecutionContext.Create false ""
    |> Context.RuntimeContext.Fake 
    |> Context.setExecutionContext
    init()     
    if args.Length = 0 then  
        Target.runOrDefault "Dev"
    else 
        Target.runOrDefault args[0] 
    0