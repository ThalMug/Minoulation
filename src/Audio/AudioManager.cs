using Godot;
using System;

public partial class AudioManager : Node
{
	private static AudioManager _instance;
	[Export] private AudioStreamPlayer2D MusicAudioPlayer;

	[Export] private AudioStreamPlayer2D SfxAudioPlayer;

	[Export] private AudioStreamWav MoveSfx;
	
	public static AudioManager Instance
	{
		get
		{
			if (_instance == null)
			{
				GD.PrintErr("instanceee");
				_instance = new AudioManager();
			}

			return _instance;
		}
	}
	
	public override void _Ready()
	{
		_instance = this;
		MusicAudioPlayer.Finished += PlayMusic;
	}

	private void PlayMusic()
	{
		MusicAudioPlayer.Play();
	}

	public void PlayMoveSfx()
	{
		SfxAudioPlayer.Stream = MoveSfx;
		SfxAudioPlayer.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
