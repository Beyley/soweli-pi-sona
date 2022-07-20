using ChaiFoxes.FMODAudio;
using FMOD;
using sowelipisona.Effects;

namespace sowelipisona.Fmod.Effects; 

public class FmodReverbAudioEffect : ReverbAudioEffect {
	private DSP _dsp;
	private int _dspIndex;
	public FmodReverbAudioEffect(AudioStream stream) : base(stream) {}

	private FmodAudioStream AsFmodAudioStream() => (FmodAudioStream)this.Stream;

	public override double ReverbDropoff {
		get => this._reverbDropoff;
		set {
			if (!this.Applied)
				throw new Exception("You must apply the effect before changing its parameters.");
			
			this.CheckDropoff(value);

			this._reverbDropoff = value;
			this._dsp.setParameterFloat((int)DSP_SFXREVERB.WETLEVEL, (float)this.ReverbDropoff);
		}
	}
	public override double ReverbTime {
		get => this._reverbTime;
		set {
			if (!this.Applied)
				throw new Exception("You must apply the effect before changing its parameters.");

			this.CheckTime(value);

			this._reverbTime = value;
			this._dsp.setParameterFloat((int)DSP_SFXREVERB.EARLYDELAY, (float)this.ReverbTime);
		}
	}
	
	protected override void InternalApply() {
		if (this.Applied)
			throw new Exception("You cannot apply an effect twice!");

		FmodAudioStream stream = this.AsFmodAudioStream();

		CoreSystem.Native.createDSPByType(DSP_TYPE.SFXREVERB, out this._dsp);
		
		this._dsp.setParameterFloat((int)DSP_SFXREVERB.EARLYDELAY, (float)this.ReverbTime);
		this._dsp.setParameterFloat((int)DSP_SFXREVERB.WETLEVEL, (float)this.ReverbDropoff);

		this._dspIndex = stream.DspIndex++;
		stream.Channel.addDSP(this._dspIndex, this._dsp);
	}
	protected override void InternalRemove() {
		if (!this.Applied)
			throw new Exception("You must apply the effect first!");
		
		FmodAudioStream stream = this.AsFmodAudioStream();
		
		stream.Channel.removeDSP(this._dsp);
	}
}
