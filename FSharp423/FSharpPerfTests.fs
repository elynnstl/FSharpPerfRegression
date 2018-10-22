namespace FSharpTests

open Microsoft.FSharp.Control
open Xunit

module FSharpPerfTests =

    [<assembly: CollectionBehavior(DisableTestParallelization = true, MaxParallelThreads = 1)>]
    do
        ()

    [<Fact>]
    let postAndReply() =
        let agent = 
            MailboxProcessor.Start(
                    (fun mbox -> 
                        async {
                            while true do
                                let! msg = mbox.Receive()
                                match msg with
                                | (message, chnl:AsyncReplyChannel<unit>) -> 
                                    chnl.Reply(())
                        }
                    ))

        for i in 1 .. 10000 do
            agent.PostAndReply(fun chnl -> ("hello", chnl))

    [<Fact>]
    let postAndReplyWithCancellation() =
        let agent = 
            MailboxProcessor.Start(
                    (fun mbox -> 
                        async {
                            while true do
                                let! msg = mbox.Receive()
                                match msg with
                                | (message, chnl:AsyncReplyChannel<unit>) -> 
                                    chnl.Reply(())
                        }
                    ), Async.DefaultCancellationToken)

        for i in 1 .. 10000 do
            agent.PostAndReply(fun chnl -> ("hello", chnl))
