using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using System.Text.RegularExpressions;
namespace teleBot
{
    public enum mode { basicmode, smartmode }
    [Serializable]
    public class botmanager
    {
        public mode BOTmode = mode.smartmode;//means anaylsis the message
        public data mydata = new data();
        string lang;
        string password;
        string username;
        bool logedin = false;
        List<long> leaders = new List<long>();
        public bool gender = true;
        public string name = "medo";
        public botmanager(string username, string password)
        {
            this.username = username;
            this.password = password;
            logedin = true;

        }
        public string Name {
            get { return name; }
            set { name = value; }
        }
        public void setlang(string lang)
        {
            this.lang = lang;
            mydata.setlang(lang);
        }
        public void login(string username, string password)
        {
            logedin = (this.username == username && this.password == password);
            if (logedin)
            {
                Console.WriteLine("Loged in successfully ! ");
            }
            else
            {
                Console.WriteLine("Invalid username or password ! ");
            }
        }
        public string recievereply(string msg, User u, Update up, bool mention = true)
        {
            try
            {
                mydata.setlang(lang);
                string ask = mydata.getresponse(msg, u, this, up ,mention);
                return ask;
            }
            catch(Exception ex)
            {
                var l=ex.Message;
                return "";
            }
        }
        public string sgender { get { if (gender) return "Male"; else return "female"; } }
        int passlen = 20;
        string pass;
        public string getpass()
        {
            string alltext = "asdfghjklzxcvbnmqwertyuiop1234567890";
            Random rd = new Random();
            pass = "";
            for (int i = 0; i < passlen; i++)
            {
                pass += alltext.Substring(rd.Next(0, alltext.Length), 1);
            }
            Console.WriteLine("new pass : " + pass);
            return pass;
        }
        public string addleader(string pss, string id)
        {
            var id2 = Convert.ToInt64(id);
            if (check(id2))
            {
                return mydata.getinfomessage(1);//leader already exists
            }
            else
            {
                if (pass != pss)
                {
                    return mydata.geterrormessage(1);//invalid password
                }
                else
                {
                    getpass();
                    leaders.Add(id2);
                    return mydata.getsuccessmessage(1);//leader added successfully
                }
            }
        }
        public string addleaderwithoutcopy(string pss, string id)
        {
            int id2 = Convert.ToInt32(id);
            if (check(id2))
            {
                return mydata.getinfomessage(1);//leader already exists
            }
            else
            {
                if (pass != pss)
                {
                    return mydata.geterrormessage(5);//invalid password
                }
                else
                {
                    getpass();
                    leaders.Add(id2);
                    return mydata.getsuccessmessage(1);//leader added successfully
                }
            }
        }
        public string setgender(bool g)
        {
            gender = g;
            return mydata.getsuccessmessage(4);//gender modified successfully
        }

        string removeleader(string pss, int id)
        {
            if (!check(id))
            {
                return mydata.geterrormessage(2);//leader doesn't exist
            }
            else
            {
                if (pass != pss)
                {
                    return mydata.geterrormessage(1);//invalid password
                }
                else
                {
                    getpass();
                    for (int i = 0; i < leaders.Count; i++)
                    {
                        if (leaders[i] == id)
                        {
                            leaders.RemoveAt(i);
                            break;
                        }
                    }
                    return mydata.getsuccessmessage(2);//leader removed successfully
                }
            }
        }
        public bool check(long ch)
        {
            bool accept = false;
            foreach (var l in leaders)
            {
                if (l == ch)
                {
                    accept = true;
                    break;
                }
            }
            return accept;
        }
        public string addreply(string request, string reply, int ch, double acceptance = 0.5)
        {

            if (check(ch))
            {
                return mydata.getsuccessmessage(0);//reply added
            }
            else
            {
                return mydata.geterrormessage(0);//0 only admin users can do this ...
            }
        }

        public void saveme(string datapath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream writerFileStream =
                 new FileStream(datapath, FileMode.Create, FileAccess.Write);
            // Save our dictionary of friends to file
            bool k = logedin;
            logedin = false;
            formatter.Serialize(writerFileStream, this);
            logedin = k;
            // Close the writerFileStream when we are done.
            writerFileStream.Close();

        }

        public static botmanager load(string datapath)
        {
            if (!System.IO.File.Exists(datapath))
            {
                Console.WriteLine("Create username : ");
                string u = Console.ReadLine();
                string pass;
                while (true)
                {
                    Console.WriteLine("password : ");
                    string up = Console.ReadLine();//.ToString();
                    Console.WriteLine("confirm password : ");
                    string up2 = Console.ReadLine();//.ToString();
                    if (up == up2)
                    {
                        pass = up;
                        Console.WriteLine("user created successfully ! ");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid password ! ");
                    }
                }
                botmanager bc = new botmanager(u, pass);
                bc.saveme(datapath);
                return bc;
            }
            botmanager tc;
            FileStream readerFileStream = new FileStream(datapath,
        FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            // Reconstruct information of our friends from file.
            tc = (botmanager)formatter.Deserialize(readerFileStream);
            // Close the readerFileStream when we are done
            readerFileStream.Close();
            return tc;
        }
    }
    [Serializable]
    public class user
    {
        int[] emotions = new int[3];//happyness_level,anger_calm,
        int[] morals = new int[1];//morality level
        int language;
        int id;
        string username;
    }
    [Serializable]
    public class genderuser
    {
        public int id;
        public bool gender;
        public genderuser(int id, bool gender)
        {
            this.id = id;
            this.gender = gender;
        }
    }
    [Serializable]
    public class timedresponse{
        public int secs = 30;
        public response resp;
        public  int nexttime = 0;
        public long chatid=0;
        public timedresponse(int secs,response resp,long chatid)
        {
            this.secs = secs;
            this.resp = resp;
            this.chatid = chatid;
        }
        public string activate(int now_secs,User u, botmanager Bot,data dt)
        {
            if (now_secs > nexttime)
            {
                nexttime = now_secs + secs;
                //resp.runme(u, Bot, dt, resp.key);
                return resp.getresponse(u,Bot,dt,null,"");
            }
            return "";
        }
    }
    [Serializable]
    public class data
    {
        public List<response> resps;
        List<string[]> firstreplaces = new List<string[]>();
        Dictionary<string, int> languages = new Dictionary<string, int>();
        Dictionary<int, List<response>> langresponses = new Dictionary<int, List<response>>();
        Dictionary<int, List<string>> errmessages = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> successmessages = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> infomessages = new Dictionary<int, List<string>>();
        Dictionary<int, List<string[]>> firstreplaces_l = new Dictionary<int, List<string[]>>();
        List<genderuser> genderlist = new List<genderuser>();
        public List<string> errs;
        public List<string> sucess;
        public List<string> infos;
        int language;
        double acceptance;
        List<string> females=new List<string>();
        public List<timedresponse> timreqs = new List<timedresponse>();
        public string addtimereq(string key,string timesecs="30",string chatid="0")
        {
            foreach (var s in resps) {
                if (s.uniquekey == key) {
                    timedresponse timreq = new timedresponse(Convert.ToInt32(timesecs), s,Convert.ToInt64(chatid));
                    timreqs.Add(timreq);
                    return getsuccessmessage(6);//timed request added successfully
                    }
            }
            return getinfomessage(2);//request now found
        }
        public string lasttimed_request { get {
                string ans = "";
                if (timreqs.Count > 0)
                {
                    ans = timreqs[timreqs.Count - 1].resp.request;
                }
                return ans;
            } }

        public string lasttimed_reply
        {
            get
            {
                string ans = "";
                if (timreqs.Count > 0)
                {
                    ans = timreqs[timreqs.Count - 1].resp.reply;
                }
                return ans;
            }
        }
        public string stime { get {
                string ans = "";
                if (timreqs.Count > 0)
                {
                    ans = timreqs[timreqs.Count - 1].secs.ToString();
                }
                return ans;
            } }
        public data()
        {
           // languages.Add("Arabic", 0);
           // languages.Add("English", 1);
            acceptance = 0.51;
        }
        public void setlang(string key)
        {
            try
            {
                language = languages[key];
                resps = langresponses[language];
                errs = errmessages[language];
                sucess = successmessages[language];
                infos = infomessages[language];
                firstreplaces = firstreplaces_l[language];
            }
            catch(Exception ex)
            {
                var s=ex.Message;
                languages.Add(key, languages.Count);
                int k = languages.Count-1;
                langresponses.Add(k, new List<response>());
                errmessages.Add(k, new List<string>());
                successmessages.Add(k, new List<string>());
                infomessages.Add(k, new List<string>());
                firstreplaces_l.Add(k,new List<string[]>());
                setlang(key);
            }
        }
        public bool checkGender(User u)
        {
            foreach(var k in genderlist)
            {
                if (k.id == u.Id)
                {
                    return k.gender;
                }
            }
            string username = u.Username.Trim(' ');
            string firstname = u.FirstName.Trim(' ');
            string lastname = u.FirstName.Trim(' ');
            foreach(var k in firstreplaces)
            {
                username = username.Replace(k[0], k[1]);
                firstname = username.Replace(k[0], k[1]);
                lastname = username.Replace(k[0], k[1]);
            }
            
            List<string> checks=new List<string>();
            checks.Add(username);
            checks.Add(firstname);
            checks.Add(lastname);
            for(int i = 0; i < checks.Count; i++)
            {

                for(int i2 = 0; i2 < checks[i].Length; i2++)
                {
                    char ch = Convert.ToChar(checks[i].Substring(i2, 1));
                    if (!char.IsLetter(ch))
                    {
                        checks[i] = checks[i].Replace(checks[i].Substring(i2,1), "");
                    }
                }
            }
            foreach (var s in females)
            {
                Regex rg = new Regex(s);
                foreach(var c in checks)
                {
                    if (rg.Match(c).Success)
                    {
                        return false;
                    }
                }
            }
            return true;// not female
        }
        string mainreq ="";
        public string translator(string msg, User u, botmanager Bot,Update up,string userrequest="",int mode=0,bool withoutreplace=false,string request="",int errmessage=-1)
        {
            /*
            rules
            'respose'{}
            'data'{}
            'botmanager'{}
            between curly brackets we write a function linked to specified class
            'class'[] get info from class
            <> we write parameters <^>means extract parameters from request message , else write the parameter directly
            (& , &) means chose according to gender of bot like I'm boy , I'm girl
            ($ , $) means chose according to gender of sender
            */
            iduser = u.Id;  
           // mainu = u;
            mainbot = Bot;
            mainreq = userrequest;
            try
            {
                Regex rbrackets=new Regex("");
                string fmatch = "";
                if (mode == 0)
                {
                    rbrackets = new Regex(@"'(\w|| )+'\{\w+\}<.*>");
                }
                else if (mode == 1)
                {
                    rbrackets = new Regex(@"'\w+'\[\w+\]");
                }
                else if (mode == 2)
                {
                    rbrackets = new Regex(@"\(\&.+\&\)");
                }
                else if (mode == 2)
                {
                    rbrackets = new Regex(@"\(\$.+\$\)");
                }
                fmatch = rbrackets.Match(msg).Value;
                if (fmatch != "" && mode == 1)
                {
                    Regex rg = new Regex(@"'\w+'");
                    string fclass = rg.Match(fmatch).Value.ToLower().Trim('\'');
                    rg = new Regex(@"\[\w+\]");
                    string prop = translator(rg.Match(fmatch).Value.Trim('[').Trim(']'), u, Bot,up);
                    switch (fclass)
                    {
                        case "user":
                            var f = u.GetType().GetProperty(prop).GetValue(u, null);
                            msg = msg.Replace(fmatch, f.ToString());
                            break;
                        case "data":
                            var f2 = this.GetType().GetProperty(prop).GetValue(this, null);
                            msg = msg.Replace(fmatch, f2.ToString());
                            break;
                        case "bot":
                            var f3 = Bot.GetType().GetProperty(prop).GetValue(Bot, null);
                            msg = msg.Replace(fmatch, f3.ToString());
                            break;
                    }
                }
                else if (fmatch != "" && mode == 0)
                {
                    Regex rg = new Regex(@"'\w+'");
                    string fclass = rg.Match(fmatch).Value.ToLower().Trim('\'');
                    rg = new Regex(@"\{\w+\}<.*>");
                    var rg2 = new Regex(@"\{\w+\}");
                    string vel = translator(rg.Match(fmatch).Value, u, Bot,up,withoutreplace:true);
                    string vk = rg2.Match(vel).Value;
                    vel = vel.Replace(vk, "");
                    var paramrg = new Regex("<.+>");
                    string paramsp = paramrg.Match(vel).Value.Trim('<').Trim('>');
                    string[] allparams = paramsp.Split(',');
                    int iid = 0;
                    var p2 = get_req_params(request, userrequest, u, Bot,up);
                    for (int i = 0; i < allparams.Length; i++)
                    {
                        if (allparams[i].Trim('^') != allparams[i])
                        {
                            allparams[i] = p2[iid];
                            iid++;
                        }
                        else
                        {
                            allparams[i] = (string)constant_params(allparams[i], u, Bot, this,up);
                        }
                    }
                    string prop = vk.Trim('{').Trim('}');
                    if (allparams.Length==1 && paramsp == "")
                    {
                        allparams = null;
                    }
                    switch (fclass)
                    {
                        case "user":
                            var f = u.GetType().GetMethod(prop);
                            return translator( (string)f.Invoke(u, allparams),u,Bot,up,userrequest,0,withoutreplace,request,errmessage);
                            break;
                        case "data":
                            var f2 = this.GetType().GetMethod(prop);
                            var s = f2.Invoke(this, allparams);
                            if (s.GetType()==(new int()).GetType())
                            {
                                return translator( msg.Replace(fmatch, ""),u,Bot,up,userrequest,0,withoutreplace,request,errmessage);
                            }
                            return (string)s;// translator(msg.Replace(fmatch,(string)s), u, Bot, userrequest, 0, withoutreplace, request, errmessage);
                            break;
                        case "bot":
                            var f3 = Bot.GetType().GetMethod(prop);
                            return translator((string)f3.Invoke(Bot, allparams), u, Bot,up, userrequest, 0, withoutreplace, request, errmessage);
                            break;
                    }
                }
                else if(mode==2 && fmatch != "")
                {

                    var rk = fmatch.Substring(2, fmatch.Length - 4).Split(',');
                    string ans = "";
                    if (Bot.gender)
                    {
                        ans = translator(rk[0],u,Bot,up,userrequest,0,withoutreplace,request,errmessage);
                    }
                    else
                    {
                        ans = translator(rk[1], u, Bot,up, userrequest, 0, withoutreplace, request,errmessage);
                    }
                    msg = msg.Replace(fmatch, ans);
                }

                else if (mode == 3 && fmatch != "")
                {

                    var rk = fmatch.Substring(2, fmatch.Length - 4).Split(',');
                    string ans = "";
                    var bb = checkGender(u);
                    if (bb)
                    {
                        ans = translator(rk[0], u, Bot,up, userrequest, 0, withoutreplace, request,errmessage);
                    }
                    else
                    {
                        ans = translator(rk[1], u, Bot,up, userrequest, 0, withoutreplace, request,errmessage);
                    }
                    msg = msg.Replace(fmatch, ans);
                }
                else
                {
                    if (!withoutreplace && mode==0)
                    {
                        Regex rg = new Regex(@"~\w+~");
                        var s = rg.Match(msg).Value;
                        if (s != "")
                        {
                            msg = msg.Replace(s, "");
                        }

                    }
                }
                if (fmatch != "")
                {
                    return translator(msg, u, Bot,up,userrequest,request:request,errmessage:errmessage);
                }
                else if (mode <3)
                {
                    return translator(msg, u, Bot,up,userrequest,mode:mode+1,withoutreplace:withoutreplace,request:request,errmessage:errmessage);
                }
                
            }
            catch(Exception ex)
            {
                string msag = ex.Message;
                if (errmessage == -1) { 
                return geterrormessage(4);//transilation error
                }
                else
                {
                    return geterrormessage(errmessage);
                }

            }
            return  msg;
        }
        //User mainu;
        long iduser;
        botmanager mainbot;
        public int openresponse(string key,string id)
        {
            User mainu = new User();
            mainu.Id = iduser;
            foreach(var r in resps)
            {
                r.openme(mainu, mainbot, this, key);
            }
            return 0;
        }
        public int runresponse(string key, string id)
        {
            User mainu = new User();
            mainu.Id = iduser;
            foreach (var r in resps)
            {
                r.runme(mainu, mainbot, this, key);
                if (r.run)
                {
                    return 0;// r.getresponse(mainu, mainbot, this,null, mainreq);
                }
            }
            return 0;
        }
        public string getrunresponse(User u)
        {
            User mainu = new User();
            mainu.Id = iduser;
            foreach (var r in resps)
            {
                if (r.run && u.Id==r.id)
                {
                    return r.getresponse(mainu, mainbot, this, null, mainreq); 
                }
            }
            return "";
        }
        public object constant_params(string cons, User u, botmanager bot, data dt,Update up)
        {
            switch (cons)
            {
                case "id":
                    return u.Id.ToString();
                case "chatid":
                    return up.Message.Chat.Id.ToString();
                default:
                    return dt.translator(cons, u, bot,up);
            }
            return cons;
        }
        public List<string> get_req_params(string req,string userrequest,User u,botmanager Bot,Update up)
        {
            try
            {
                Regex rg = new Regex("~.+~");
                string kk = translator(req, u, Bot,up, userrequest: userrequest, withoutreplace: true);
                string gf = kk;
                var paramst = rg.Match(gf).Value;
                var paramst12 = gf.Replace(paramst, "").Trim(' ');
                Regex rggf = new Regex("");
                if (paramst12.Length > 4)
                {
                    rggf = new Regex(paramst12.Substring(paramst12.Length - 2, 2).ToLower() + ".+");
                }
                else
                {
                    rggf = new Regex(paramst12.Substring(paramst12.Length).ToLower() + ".+");
                }
                string[] paramst2 = { "" };
                if (paramst12.Length > 4)
                {
                    try
                    {
                        paramst2 = rggf.Match(userrequest.ToLower(), paramst12.Length - 4).Value.Substring(2).Split(',');
                    }
                    catch { }
                }
                else
                {
                    
                    paramst2[0]= userrequest;
                }
                var par = paramst.Trim('~').Split(',');
                List<string> pl = new List<string>();
                foreach (var p in paramst2)
                {
                    pl.Add(p.Trim(' '));
                }
                return pl;
            }
            catch
            {
                return null;
            }
            
        }
        public string makegender(int id,bool gender)
        {
            
            foreach(var c in genderlist)
            {
                if (c.id == id)
                {
                    return getinfomessage(1);//user already exists
                }
            }
            genderlist.Add(new genderuser(id, gender));
            return getsuccessmessage(3);//user added to gender list successfully
        }
        public string geterrormessage(int indx)
        {
            User mainu = new User();
            mainu.Id = iduser;
            if (indx >= errs.Count)
            {
                return "";
            }
            return translator(errs[indx],mainu,mainbot,null);
        }
        public string getsuccessmessage(int indx)
        {
            User mainu = new User();
            mainu.Id = iduser;
            if (indx >= sucess.Count)
            {
                return "";
            }
            return translator(sucess[indx], mainu, mainbot,null);
        }
        public string getinfomessage(int indx)
        {
            User mainu = new User();
            mainu.Id = iduser;
            if (indx >= infos.Count)
            {
                return "";
            }
            return translator(infos[indx],mainu,mainbot,null);
        }
        public string addresponse(string request, string reply,string key)
        {
            response res = new response(request, reply,key);
            resps.Add(res);
            return getsuccessmessage(5);//response added successfully
        }
        string reqg = "";
        public int setrequest(string req)
        {
            reqg = req;
            return 0;
        }

        string respg = "";
        public int setreply(string req)
        {
            respg = req;
            return 0;
        }

        string rkey = "";
        public int setrkey(string req)
        {
            rkey = req;
            return 0;

        }
        public string prerequest { get { return reqg; } }
        public string preresponse { get { return respg; } }
        public string prekey { get { return rkey; } }
        public string quickaddrequest()
        {
           // if (ans=="no" || ans == "لا")
            {
             //   return getinfomessage(3);// request canceled
            }
            addresponse(reqg, respg, rkey);
            return getsuccessmessage(5);//request added
        }
        public int addmoretoresponse(string keyh,string request="",string reply="")
        {
            int id = -1;
            int x = 0;
            foreach(var l in resps)
            {
                if (l.uniquekey.ToLower() == keyh)
                {
                    id = x;
                    break;
                }
                x++;
            }
            if (id == -1)
            {
                return 0;// "";
            }
            try
            {

                var resp = resps[id];
                if (request != "")
                {
                    resp.addrequest(request);
                }
                if (reply != "")
                {
                    resp.addresponse(reply);
                }

            }
            catch
            {
                return 0;// geterrormessage(5);//couldn't modify response
            }
            return 0;// getsuccessmessage(3);//response modified successfully
        }
        public string getresponse(string msg,User u, botmanager Bot,Update up, bool mention = true)
        {
            foreach( var f in firstreplaces)
            {
                msg=msg.Replace(f[0], f[1]);
            }
            double best = -10000000000;
            response best1=new response("","","");
            foreach (var k in resps)
            {
                var q = k.matcchpercentage(msg,u,Bot,this,up,mention);
                if (q > best)
                {
                    best1 = k;
                    best = q;
                }
            }
            if (best > acceptance)
            {
                return best1.getresponse(u,Bot,this,up,msg);
            }
            return "";
        }
    }
    [Serializable]
    public class response
    {
        public string request;
        public string reply;
        public string key="";//when it matches add value to percentage
        string regexmatch;
        List<string> equivelant_requests = new List<string>();
        List<string> equivelant_keys = new List<string>();
        List<string> list_respone = new List<string>();
        public int language;
        double acceptance=0.5;
        public int response_rule = 0;//0 means randomly chose a response when any request matches,1 means equivelant response
        List<string[]> replaces=new List<string[]>();
        public int type = 0;//0 means requires mention , 1 means public ,2 means private requires open to be reached
        public bool leader_resp=false;
        public string match_request;
        public bool open;
        public bool run;
        public long id;
        public int Match_index=-1;
        public string uniquekey;
        public int keytype = 1;//default public
        public int errmessage = -1;
        public void setkey(string pkey)
        {
            key = pkey;
        }
        public void seterrmessage(string idx)
        {
            errmessage = Convert.ToInt32(idx);
        }
        public response(string request,string reply,string uniquekey,double acceptance=0.5)
        {
            this.request = request;
            this.reply = reply;
            this.acceptance = acceptance;
            this.uniquekey = uniquekey;
            string[] k = { " ", "" };
            replaces.Add(k);
        }
        public void openme(User u, botmanager Bot, data dt,string matkey)
        {
            if (matkey.Substring(0,matkey.Length-2).ToLower() == uniquekey.ToLower())
            {
                open = true;
                id = u.Id;
                Match_index =Convert.ToInt32(matkey.Substring(matkey.Length - 2, 2));
            }
        }
        public void runme(User u, botmanager Bot, data dt, string matkey)
        {
            if (matkey.Substring(0, matkey.Length - 2) == uniquekey)
            {
                run = true;
                id = u.Id;
                Match_index = Convert.ToInt32(matkey.Substring(matkey.Length - 2, 2));
            }
        }
        public string getresponse(User u, botmanager Bot,data dt,Update up,string userrequest,bool isleader=false)
        {
            isleader = Bot.check(Convert.ToInt64( u.Id));
            
            if (leader_resp && !isleader)
            {
                
                return dt.geterrormessage(0);//onlyleader can access this
            }
            if (list_respone.Count > 0)
            {
                if (response_rule == 0)
                {
                    Random rd = new Random();
                    int idx = rd.Next(0, list_respone.Count + 1);
                    if (idx < list_respone.Count)
                    {
                        return dt.translator(list_respone[idx], u, Bot,up, userrequest: userrequest, request: match_request,errmessage:errmessage);
                    }
                }
                else if (response_rule== 1)
                {
                    if (Match_index != -1 && Match_index<list_respone.Count)
                    {
                        return dt.translator(list_respone[Match_index], u, Bot,up, userrequest: userrequest, request: match_request,errmessage:errmessage);
                    }
                }
            }
            return dt.translator(reply,u,Bot,up,userrequest:userrequest, request:match_request,errmessage: errmessage);
        }
        public double matcchpercentage(string msg,User u,botmanager bot,data dt,Update up, bool mention = true)
        {
            if ((type ==0 && !mention && keytype==0)|| (type==2 && ! open && ! run))
            {
                return 0;
            }
            double adds = 0;

            double sf = 0;// compare(msg, key, u, bot, dt);
            Regex rgkey = new Regex("" + dt.translator(key,u,bot,up,msg).ToLower().Trim() + "");
            if (key!="" && rgkey.Match(msg.ToLower()).Success)
            {
                sf = 0.6;
            }
            if (sf > 0.5)
            {
                
                adds += 0.5 + sf;
            }
            else if (type == 0 && !mention)
            {
                return 0;
            }
            if (open && id==u.Id)
            {
                open = false;
                if (Match_index == -1 || Match_index>equivelant_requests.Count)
                {
                    match_request = request;
                }
                else if (Match_index == -2)
                {
                    open = false;
                    int tt = type;
                    type = 1;
                    var k= matcchpercentage( msg,  u,  bot,  dt,  up,  mention )+100.3;
                    type = tt;
                    return k;
                }
                else
                {
                    match_request = equivelant_requests[Match_index];
                }
                return 1000.3+adds;
            }
            run = false;
            double bestmatch = 0;
            var sc = compare(msg, request, u, bot, dt,up);
            if ( sc>= 1)
            {
                Match_index = -1;
                match_request = request;
                
                return sc+adds; 
            }
            else
            {
                match_request = request;
                Match_index = -1;
                bestmatch = sc;
            }
            int i = 0;
            foreach(var c in equivelant_requests)
            {
                
                var sc2 = compare(msg, c, u, bot, dt,up);
                if (sc2 >= 1)
                {
                    match_request = c;
                    Match_index = i;
                    return sc2+adds;
                }
                else if (sc2 > bestmatch)
                {
                    bestmatch = sc2;
                    match_request = c;
                    Match_index = i;
                }
                i++;
            }
            return bestmatch+adds;
        }
        double compare(string s1,string s2,User u,botmanager bot,data dt,Update up)
        {
            if (s1 == s2)
            {
                return 3;
            }
            string trans1 = dt.translator(s1, u, bot,up, s1);
            string trans2 = dt.translator(s2, u, bot,up, s1);
            if (s1 == trans2|| s2==trans1 || trans1==trans2)
            {
                return 0.9*3;
            }
            var l1 = smartcompare(s1, s2);
            var l2 = smartcompare(s1, trans2);
            var l3 = smartcompare(s2, trans1);
            return Math.Max(l1,Math.Max( l2,l3));
        }
        public double smartcompare(string s1,string s2)
        {
            double amount = 0;
            int max = 0;
            for(int i=0;(i<s1.Length && i<s2.Length);i++)
            {
                string sa = s1.Substring(i, 1).ToLower();
                string sa2 = s2.Substring(i, 1).ToLower();
                string saa2 = "";
                if (i + 1 < s2.Length)
                {
                     saa2 = s2.Substring(i + 1, 1).ToLower();
                }
                string saa = "";
                if (i + 1 < s1.Length)
                {
                     saa = s1.Substring(i + 1, 1).ToLower();
                }
                string sb1 = "";
                if (i - 1 > 0)
                {
                    sb1 = s1.Substring(i - 1, 1);
                }
                string sb2 = "";
                if (i - 1 > 0)
                {
                    sb2 = s2.Substring(i - 1, 1);
                }
                if ( sa==sa2 || saa2==sa|| saa==sa2 || sa==sb2 || sa2==sb1)
                {
                    amount++;
                }
                
                max++;
            }
            Dictionary<char, double[]> lettersnum = new Dictionary<char, double[]>();
            foreach(var l in s1.Replace(" ",""))
            {
                var ls = Convert.ToChar(l.ToString().ToLower());
                if (lettersnum.ContainsKey(ls))
                {
                   // var c=
                    lettersnum[ls][0]++;
                }
                else
                {
                    double[] ni = { 1, 0 };
                    lettersnum.Add(ls, ni);
                }
            }
            foreach (var l in s2.Replace(" ", ""))
            {
                var ls = Convert.ToChar(l.ToString().ToLower());
                if (lettersnum.ContainsKey(ls))
                {
                    lettersnum[ls][1]++;
                }
                else
                {
                    double[] ni = { 0, 1 };
                    lettersnum.Add(ls, ni);
                }
            }
            double adds = 0;
            double sum = 0;
            double sdash = 0;
            if (s1=="" || s2 == "")
            {
                return 0;
            }
            double j = Math.Abs(s1.Length / s2.Length - s2.Length / s1.Length);
            bool kl = false;
            if ( j < 3)
            {
                kl = true;
            }
            else
            {
                adds -= 3;
            }
            foreach (var k in lettersnum)
            {
                adds += 1 - Math.Abs(k.Value[0] - k.Value[1]) / (k.Value[0] + k.Value[1]);
                sum += (k.Value[0] + k.Value[1]) / 2;
                if (sum > s1.Length || sum > s2.Length && kl)
                {
                    break;
                }
                sdash++;
            }
            adds = adds / sdash;
            if (max == 0)
            {
                max = 1;
            }
            else
            {
                max = (s1.Length + s2.Length) / 2;
                if (amount / max < 0.6)
                {
                    Regex rg = new Regex(s1.Trim());
                    if (rg.Match(s2).Success)
                    {
                        return 0.6000001+adds;
                    }
                    rg = new Regex(s2.Trim());
                    if (rg.Match(s1).Success)
                    {
                        return 0.6000001+adds;
                    }
                }
            }
            max = (s1.Length + s2.Length)/2;
            return adds;// (amount / (max)+adds)/2;
        }
        public void addresponse(string resp)
        {
            list_respone.Add(resp);
        }
        public void addrequest(string req)
        {
            equivelant_requests.Add(req);
        }
        public void remove_request(int indx)
        {
            equivelant_requests.RemoveAt(indx);
        }
        public void remove_responses(int indx)
        {
            list_respone.RemoveAt(indx);
        }
        
    }
}
