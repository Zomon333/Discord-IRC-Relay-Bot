using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TransferMessage
{
    public bool inIRC = false;
    public bool inDiscord = false;

    public string user = "UNK";
    public string channel = "UNK";
    public string source = "UNK";
    public string message = "UNK";

    #region Accessors & Mutators
    public string getPrint()
    {
        string tmp = source + " | " + channel + " | " + user + " >> " + message;
        return tmp;
    }
    public bool getIRC() { return inIRC; }
    public void setIRC(bool ninIRC) { inIRC = ninIRC; }
    public bool getDiscord() { return inDiscord; }
    public void setDiscord(bool ninDiscord) { inDiscord = ninDiscord; }
    #endregion

    public TransferMessage(string Nuser, string Nchannel, string Nsource, string Nmessage, bool NinIRC, bool NinDiscord)
    {
        user = Nuser;
        channel = Nchannel;
        source = Nsource;
        message = Nmessage;
        inIRC = NinIRC;
        inDiscord = NinDiscord;
    }
    public TransferMessage()
    {
        user = "NULL";
        channel = "NULL";
        source = "NULL";
        message = "NULL";
        inIRC = false;
        inDiscord = false;
    }
}