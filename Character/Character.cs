using Godot;
using System;

public partial class Character : CharacterBody3D
{
    private const float Speed = 5f;
    private const float Gravity = 30f;
    private const float Acceleration = 0.5f;
    private const float Sensitivity = 0.001f;

    public Node3D Head;
    public Camera3D Camera;


    private Vector3 velocity = Vector3.Zero;

    public override void _Ready()
    {
        Head = GetNode<Node3D>("Head");
        Camera = GetNode<Camera3D>("Head/Camera");
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion eventMouseMotion)
        {
            InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
            Head.RotateY(-mouseMotion.Relative.X * Sensitivity);
            Camera.RotateX(-mouseMotion.Relative.Y * Sensitivity);

            Vector3 cameraRot = Camera.Rotation;
            cameraRot.X = Mathf.Clamp(cameraRot.X,Mathf.DegToRad(-80f), Mathf.DegToRad(80f));
            Camera.Rotation = cameraRot;
        }

        if (Input.IsKeyPressed(Key.Escape))
        {
            GetTree().Quit();
        }
    }
    
    public override void _PhysicsProcess(double delta)
    {
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        Vector3 direction = (Head.GlobalTransform.Basis * new Vector3(inputDirection.X, 0 , inputDirection.Y)).Normalized();

        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0 , Acceleration);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0 , Acceleration);
        }

        if (!IsOnFloor())
        {
            velocity.Y -= Gravity * (float)delta;
        }
        else
        {
            velocity.Y = 0;
        }
        
        Velocity = velocity;
        MoveAndSlide();
    }
}