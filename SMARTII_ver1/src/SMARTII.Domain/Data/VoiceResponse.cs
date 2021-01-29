using System.Xml.Serialization;

[XmlRoot("Root")]
public partial class VoiceSearchResponse
{
    private string msgField;

    private VoiceSearchData dATAField;

    /// <remarks/>
    public string msg
    {
        get
        {
            return this.msgField;
        }
        set
        {
            this.msgField = value;
        }
    }

    /// <remarks/>
    public VoiceSearchData DATA
    {
        get
        {
            return this.dATAField;
        }
        set
        {
            this.dATAField = value;
        }
    }
}

[XmlRoot("data")]
public partial class VoiceMatchResponse
{
    private string resultField;

    private string msgField;

    /// <remarks/>
    public string result
    {
        get
        {
            return this.resultField;
        }
        set
        {
            this.resultField = value;
        }
    }

    /// <remarks/>
    public string msg
    {
        get
        {
            return this.msgField;
        }
        set
        {
            this.msgField = value;
        }
    }
}

public partial class VoiceSearchData
{
    private uint callLogIDField;

    private object idField;

    private string sIDField;

    private ushort eIDField;

    private string mAIDField;

    private string mAIPField;

    private string uRLField;

    private object mIDField;

    /// <remarks/>
    public uint CallLogID
    {
        get
        {
            return this.callLogIDField;
        }
        set
        {
            this.callLogIDField = value;
        }
    }

    /// <remarks/>
    public object ID
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }

    /// <remarks/>
    public string SID
    {
        get
        {
            return this.sIDField;
        }
        set
        {
            this.sIDField = value;
        }
    }

    /// <remarks/>
    public ushort EID
    {
        get
        {
            return this.eIDField;
        }
        set
        {
            this.eIDField = value;
        }
    }

    /// <remarks/>
    public string MAID
    {
        get
        {
            return this.mAIDField;
        }
        set
        {
            this.mAIDField = value;
        }
    }

    /// <remarks/>
    public string MAIP
    {
        get
        {
            return this.mAIPField;
        }
        set
        {
            this.mAIPField = value;
        }
    }

    /// <remarks/>
    public string URL
    {
        get
        {
            return this.uRLField;
        }
        set
        {
            this.uRLField = value;
        }
    }

    /// <remarks/>
    public object mID
    {
        get
        {
            return this.mIDField;
        }
        set
        {
            this.mIDField = value;
        }
    }
}
