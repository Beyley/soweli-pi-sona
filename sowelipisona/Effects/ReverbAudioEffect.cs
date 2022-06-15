namespace sowelipisona.Effects;

public abstract class ReverbAudioEffect : AudioEffect {
    public abstract double ReverbDropoff {
        get;
        set;
    }
    public abstract double ReverbTime {
        get;
        set;
    }

    protected void CheckTime(double time) {
        if (time is < 0 or > 300)
            throw new ArgumentOutOfRangeException("Your reverb time is out of range! (0, 300)");
    }

    protected void CheckDropoff(double time) {
        if (time is > 0 or < -80)
            throw new ArgumentOutOfRangeException("Your reverb dropoff is out of range! (0, -80)");
    }
    
    protected double _reverbDropoff = 0;
    protected double _reverbTime = 250;

    protected ReverbAudioEffect(AudioStream stream) {
        this.Stream = stream;
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