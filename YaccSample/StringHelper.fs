
module StringHelper

open System

let (|InvariantEqual|_|) (str:string) arg = 
    if String.Compare(str, arg, StringComparison.OrdinalIgnoreCase) = 0
        then Some() else None

//match "HellO" with
//| InvariantEqual "hello" -> printfn "yep!"
//| _ -> printfn "Nop!" 