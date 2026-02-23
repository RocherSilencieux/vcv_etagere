using vcv_etagere;

public interface IAudioInput
{
    void Connect(IAudioNode node);
    void Disconnect();
}

