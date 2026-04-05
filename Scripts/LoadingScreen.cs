using Godot;
using System;

public partial class LoadingScreen : CanvasLayer
{
	[Export] public string NextScene = "res://scenes/MaScene.tscn";
	private VideoStreamPlayer _video;
	private bool _sceneReady = false;
	private bool _videoFinished = false;

	public override void _Ready()
	{
		_video = GetNode<VideoStreamPlayer>("VideoStreamPlayer");
		ResourceLoader.LoadThreadedRequest(NextScene);
		_video.Finished += OnVideoFinished;
		_video.Play();
		SetProcess(true);
	}

	private void OnVideoFinished()
	{
		_videoFinished = true;
		TryChangeScene();
	}

	public override void _Process(double delta)
	{
		if (!_sceneReady)
		{
			var status = ResourceLoader.LoadThreadedGetStatus(NextScene);
			if (status == ResourceLoader.ThreadLoadStatus.Loaded)
			{
				_sceneReady = true;
				TryChangeScene();
			}
		}
	}

	private void TryChangeScene()
	{
		// Change de scène seulement si les deux sont prêts
		if (_sceneReady && _videoFinished)
		{
			var scene = (PackedScene)ResourceLoader.LoadThreadedGet(NextScene);
			QueueFree();
			GetTree().ChangeSceneToPacked(scene);
		}
	}
}
