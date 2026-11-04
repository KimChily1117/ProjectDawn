namespace DnfRaidDemo
{
    public sealed class ChatEnterInput
    {
        bool held, pending;
        int requestedFrame = -1, settledFrame = -1;

        public bool Press(bool open, int frame)
        {
            if (held)
                return false;
            held = true;

            if (!open)
                return true;
            pending = true;
            requestedFrame = frame;
            settledFrame = -1;

            return false;
        }

        public void Release()
        {
            held = false;
        }

        public void Cancel()
        {
            pending = false;
            settledFrame = -1;
        }

        public bool TakeSubmit(bool composing, int frame)
        {
            if (!pending)
                return false;

            if (composing)
            {
                settledFrame = -1;

                return false;
            }

            if (settledFrame < 0)
                settledFrame = frame;

            if (frame <= requestedFrame || frame <= settledFrame)
                return false;
            pending = false;

            return true;
        }
    }
}
