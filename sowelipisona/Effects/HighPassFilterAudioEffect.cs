namespace sowelipisona.Effects;

public abstract class HighPassFilterAudioEffect : AudioEffect {
	protected double _frequencyCutoff = 1000;

	protected HighPassFilterAudioEffect(AudioStream stream) {
		this.Stream = stream;
	}
	public abstract double FrequencyCutoff {
		get;
		set;
	}

	public override void Apply() {
		this.InternalApply();

		this.Applied = true;
	}

	public override void Remove() {
		this.InternalRemove();

		this.Applied = false;
	}

	protected abstract void InternalApply();
	protected abstract void InternalRemove();
}
