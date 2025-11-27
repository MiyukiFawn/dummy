using Player.StateMachine;
using Player.StateMachine.LocomotionLayer;
using StateMachine;

namespace Player
{
    public static class PlayerFactory
    {
        public static FiniteStateMachine<PlayerBaseState> BuildStateMachine(PlayerContext context)
        {
            FiniteStateMachine<PlayerBaseState> sm = new FiniteStateMachine<PlayerBaseState>();

            StateLayer<PlayerBaseState> locomotion = new StateLayer<PlayerBaseState>("Locomotion",
                new Root(context, sm)
                    .AddChild(new Grounded(context, sm)
                        .AddChild(new Standing(context, sm)
                            .AddChild(new Idle(context, sm))
                            .AddChild(new Walk(context, sm))
                            .AddChild(new Run(context, sm))
                            .AddChild(new RunStop(context, sm))
                            .AddChild(new Spin(context, sm))
                        )
                        .AddChild(new Crouch(context, sm)
                            .AddChild(new CrouchStill(context, sm))
                            .AddChild(new Crawl(context, sm))
                        )
                    )
                    .AddChild(new Airborne(context, sm)
                        .AddChild(new Jump(context, sm))
                        .AddChild(new Fall(context, sm))
                    )
            );

            return sm
                .AddLayer(locomotion)
                .Initialize();
        }
    }
}