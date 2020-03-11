using System;
using System.Collections.Generic;
using System.Linq;
using NetOffice.VisioApi;
using SPES_App.Utility;
using SPES_Wissenskontext;
using SPES_Funktionsnetz;
using SPES_LogicalViewpoint;
using SPES_TechnicalViewpoint;
using SPES_FunktionellerKontext;
using SPES_StrukturellerKontext;
using SPES_Zielmodell;
using SPES_SzenarioUseCases;
using ITU_Scenario;
using NetOffice.VisioApi.Enums;
using SPES_StrukturellePerspektive;
using SPES_FunktionellePerspektive;
using SPES_Verhaltensperspektive;
using static SPES_App.Forms.SelectSystemFunction;
using static SPES_App.Forms.ContextFunction;
using System.Windows.Forms;
using Application = NetOffice.VisioApi.Application;
using System.Drawing;

namespace SPES_App
{
    public class SpesActivities
    {
        Application _application;

        List<IVMaster> ActiveMasters
        {
            get
            {
                List<IVMaster> masters = new List<IVMaster>();
                foreach (IVDocument doc in _application.Documents)
                    foreach (IVMaster master in doc.Masters)
                        masters.Add(master);
                return masters;
            }
        }

        public SpesActivities(Application a)
        {
            this._application = a;
        }

        public void CreateSystem(SPES_DocumentReferencer pReferencer)
        {
            IVShape systemshape = null;
            foreach (Shape s in this._application.ActivePage.Shapes)
            {
                systemshape = s;
            }
            IVPage p = this._application.ActiveDocument.Pages.Add();
            p.Name = systemshape.Text;
            CreateSystemElements(p, pReferencer);
            System.Windows.Forms.MessageBox.Show("Artifact Creation for Level 0 finished!");
        }

        public void CreateRectangle(string name)
        {
            this._application.ActivePage.Name = "System Overview";
            IVShape s= this._application.ActivePage.DrawRectangle(1, 1, 3, 1.5);
            s.Text = name; s.SetCenter((10 / 2.54), (27.5 / 2.54));
            s.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
        }

        private void CreateSystemElements( IVPage p, SPES_DocumentReferencer pReferencer)
        {
            //CellsSRC(1,11,4) gibt an, wo der Text positioniert werden soll
            //setCenter (double x, double y) positioniert das ausgewaehlte Shape an die gewuenschte Stelle, angegebene Werte
            //sind in Zoll , um von cm auf Zoll zu kommen, muss durch 2.54 dividiert werden.
            IVShape header, systemName, rvp, fvp, lvp, tvp, statusRvp, statusFvp, statusLvp, statusTvp;
            IVHyperlink rvphl, fvphl, lvphl, tvphl;
            header = p.DrawRectangle(1, 1, 8, 1.5); header.LineStyle = "none"; header.Text ="Artifacts of " + p.Name;
            header.SetCenter(4, (28/2.54));  header.CellsSRC(3, 0, 7).FormulaU = "24 pt"; header.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";

            systemName = p.DrawRectangle(1, 1, 8, 4); systemName.Text = p.Name; systemName.SetCenter(4, (23.2/2.54));
            systemName.CellsSRC(1, 11, 4).Formula = "0"; systemName.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";

            rvp = p.DrawRectangle(1, 1, 2.5, 3);rvp.Text = "Requirements Engineering Viewpoint";
            rvp.SetCenter(4.2/2.54, (22.8 / 2.54)); rvp.CellsSRC(1, 11, 4).Formula = "0";
            rvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusRvp = p.DrawOval(1, 1, 1.16, 1.16); statusRvp.SetCenter(4.2 / 2.54, 21.3 / 2.54); statusRvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

            fvp = p.DrawRectangle(1, 1, 2.5, 3); fvp.Text = "Functional Viewpoint"; fvp.SetCenter(8.2/2.54, (22.8 / 2.54));
            fvp.CellsSRC(1, 11, 4).Formula = "0"; fvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusFvp = p.DrawOval(1, 1, 1.16, 1.16); statusFvp.SetCenter(8.2 / 2.54, 21.3 / 2.54); statusFvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

            lvp = p.DrawRectangle(1, 1, 2.5, 3); lvp.Text = "Logical Viewpoint"; lvp.SetCenter(12.2/2.54, (22.8 / 2.54));
            lvp.CellsSRC(1, 11, 4).Formula = "0"; lvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusLvp = p.DrawOval(1, 1, 1.16, 1.16); statusLvp.SetCenter(12.2 / 2.54, 21.3 / 2.54); statusLvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

            tvp = p.DrawRectangle(1, 1, 2.5, 3); tvp.Text = "Technical Viewpoint"; tvp.SetCenter(16.2/2.54, (22.8 / 2.54));
            tvp.CellsSRC(1, 11, 4).Formula = "0"; tvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusTvp = p.DrawOval(1, 1, 1.16, 1.16); statusTvp.SetCenter(16.2 / 2.54, 21.3 / 2.54); statusTvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
            //System.Windows.Forms.MessageBox.Show(("Create Documents?"));
            CreateViewPointDocs(p.Name, this._application.ActiveDocument.Path, pReferencer);

            rvphl=rvp.AddHyperlink();
            rvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_RVP.vsdx"));
            fvphl = fvp.AddHyperlink();
            fvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_FVP.vsdx"));
            lvphl = lvp.AddHyperlink();
            lvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_LVP.vsdx"));
            tvphl = tvp.AddHyperlink();
            tvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_TVP.vsdx"));
 
        }

        public void CreateSheetsforMscReferences()
        {
            List<IVShape> references = new List<IVShape>();

            foreach (var s in this._application.ActivePage.Shapes)
            {
                if (s.Name.Contains("MSC Reference"))
                {
                    bool exists = false;
                    foreach (var sh in references)
                    {
                        if (sh.Text == s.Text)
                        {
                            exists = true; System.Windows.Forms.MessageBox.Show(sh.Text +
                                        " exists twice ore more as a MSC Reference.");
                        }
                    }
                    if (exists == false) { references.Add(s); }
                }
            }

            if (references.Count>=1)
            {
                this._application.Documents.OpenEx("SMT_bMSC.vssx", 4);
                foreach (var r in references)
                {
                    bool exist = false;
                    foreach(var p in this._application.ActiveDocument.Pages)
                    {
                        if (p.Name == r.Text)
                        {
                            exist = true;
                            System.Windows.Forms.MessageBox.Show(r.Text +
                                        " already exists.");
                        }
                    }
                    if (exist==false)
                    {
                        IVPage p = this._application.ActiveDocument.Pages.Add();
                        p.Name = r.Text;
                        IVHyperlink hl = r.Hyperlinks.Add();
                        hl.SubAddress = p.Name;
                    }
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No MSC Reference found.");
            };
        }

        private void CreateViewPointDocs(string systemname, string path, SPES_DocumentReferencer pReferencer)
        {

            using (Application app = new Application())
            {
                Application subapplic = this._application;
                IntPtr appkey = new IntPtr(0);
                IntPtr helpappkey = new IntPtr(0);
                Application applic = null;
                foreach (var window in OpenWindowGetter.GetOpenWindows())
                {
                    if (window.Value.Contains("Visio Professional"))
                    {                      
                        OpenWindowGetter.SetForegroundWindow(window.Key);
                        applic = NetOffice.VisioApi.Application.GetActiveInstance();                     
                        if (app == applic) { helpappkey = window.Key; }
                        else if (applic == this._application) { appkey = window.Key; };
                    };
                }
                OpenWindowGetter.SetForegroundWindow(helpappkey);
                CreateemptyModels(app, path, systemname, pReferencer);
                var doc = app.Documents.Add("");
                CreateRvp(systemname, doc);
                doc.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP.vsdx"));
                doc.Close();
                
                doc = app.Documents.Add("");
                CreateFvp(systemname, doc);
                //app.Documents.OpenEx("SMT_FN_Funktionsnetz.vssx", 4); app.Documents.OpenEx("SMT_IA.vssx", 4);
                doc.SaveAs(System.IO.Path.Combine(path, systemname + "_FVP.vsdx"));
                doc.Close();
                pReferencer.AddAssignment(systemname + "_FVP.vsdx", typeof(FunktionsnetzNetwork).Name);

                doc = app.Documents.Add("");
                CreateLvp(systemname, doc);
                //app.Documents.OpenEx("SMT_Class.vssx", 4); 
                doc.SaveAs(System.IO.Path.Combine(path, systemname + "_LVP.vsdx"));
                doc.Close();
                pReferencer.AddAssignment(systemname + "_LVP.vsdx", typeof(LogicalViewpointNetwork).Name);

                doc = app.Documents.Add("");
                CreateTvp(systemname, doc);
                //app.Documents.OpenEx("SMT_SM.vssx", 4);
                //app.Documents.OpenEx("SMT_IA.vssx", 4);
                doc.SaveAs(System.IO.Path.Combine(path, systemname + "_TVP.vsdx"));
                doc.Close();
                pReferencer.AddAssignment(systemname + "_TVP.vsdx", typeof(TechnicalViewpointNetwork).Name);

                OpenWindowGetter.SetForegroundWindow(appkey);
                app.Quit();
            }
            
        }

        public void SystemFunctiontoPage()
        {
            List<IVShape> shapes = new List<IVShape>();
            foreach (IVShape shape in this._application.ActivePage.Shapes)
            {
                if (shape.Name.Contains("System Function"))
                {
                    bool exists = false;
                    foreach (var s in shapes)
                    {
                        if (s.Text == shape.Text)
                        {
                            exists = true; System.Windows.Forms.MessageBox.Show(shape.Text +
    " already exists");
                        }
                    }
                    if (exists == false) { shapes.Add(shape); }
                }

            }
            if (shapes.Count >= 1)
            {
                IVDocument stencil = this._application.Documents.OpenEx("SMT_IA.vssx", 4);
                IVMaster masterboundary = new IVMaster();
                IVMaster masterstate = new IVMaster();
                foreach (var m in stencil.Masters)
                {
                    if (m.Name == "Interface")
                    {
                        masterboundary = m;
                    }
                    else if (m.Name == "Initial State")
                    {
                        masterstate = m;
                    }
                }
                foreach (var shape in shapes)
                {
                    IVPage page = this._application.ActiveDocument.Pages.Add();
                    IVShape shapeh = page.Drop(masterboundary, 10.3 / 2.54, 20.5 / 2.54);
                    shapeh.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                    IVShape shapeis = page.Drop(masterstate, 11.3 / 2.54, 18.5 / 2.54);
                    shapeis.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";

                    page.Name = shape.Text;
                    IVHyperlink hl = shape.Hyperlinks.Add();
                    hl.SubAddress = page.Name;
                }
            }

        }

        public void createInstanceModel()
        {
                drawInstanceModel();  
        }

        private void drawInstanceModel()
        {


            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            form.Text = "Select Configuration Type";
            RadioButton rbutton1 = new RadioButton();
            RadioButton rbutton2 = new RadioButton();
            rbutton1.Text = "Configuration of goal model based on configuration";
            rbutton2.Text = "Configuration of DCM based on goal selection";
            rbutton1.Size = new Size(300, 15);
            rbutton2.Size = new Size(380,15);
            rbutton1.Location = new Point(20, 20);
            rbutton2.Location = new Point(20, 60);
            Button button1 = new Button();
            button1.Text = "OK";
            button1.Location = new Point(100, 100);
            form.Controls.Add(rbutton1);
            form.Controls.Add(rbutton2);
            form.Controls.Add(button1);
            form.AcceptButton = button1;
            form.CancelButton = button1;
            form.Size = new Size(400,200);
            form.ShowDialog();
            bool open = true;
            while (open)
            {
                if (form.Visible == false)
                {
                    open = false;
                }
            }
            if (rbutton1.Checked)
            {
                System.Windows.Forms.Form form2 = new System.Windows.Forms.Form();
                form2.Text = "Set Configuration";
                Button button5 = new Button();
                button5.Text = "OK";

                List<TextBox> texte = new List<TextBox>();
                List<Label> labels = new List<Label>();
                int point = 20;
                int counter = 0;
                foreach (var shape in this._application.ActivePage.Shapes)
                {
                    if (shape.Name.Contains("System Type"))
                    {
                        Label label = new Label();
                        label.Text = shape.Text;
                        label.Location = new Point(10, point);
                        TextBox box = new TextBox();
                        box.Location = new Point(100, point);
                        form2.Controls.Add(box);
                        form2.Controls.Add(label);
                        texte.Add(box);
                        labels.Add(label);
                        point += 35;
                        counter++;
                    }
                }
                button5.Location = new Point(200, point);
                form2.Controls.Add(button5);
                double size = (double)point * 1.5;
                form2.Size = new Size(350, Convert.ToInt32(size));
                form2.AcceptButton = button5;
                form2.CancelButton = button5;

                String[,] values = new string[2, counter];
                DialogResult dr = form2.ShowDialog();
                if (DialogResult.Cancel == dr)
                {

                    for (int i = 0; i < counter; i++)
                    {
                        values[0, i] = labels[i].Text;
                        values[1, i] = texte[i].Text;
                    }
                    
                }
                IVPage activepage = this._application.ActivePage;
                IVPage pastedpage = selectShapesofGRL(activepage, values);
                List<IVShape> unreachablegoals= CheckConfiguration(activepage, pastedpage, values);
                if (unreachablegoals.Count != 0)
                {
                    deleteshapes(unreachablegoals, pastedpage);
                }
                
            }
            else if (rbutton2.Checked)
            {
                IVPage activepage = this._application.ActivePage;
                IVSelection selection = this._application.ActiveWindow.Selection;
                IVPage pastedpage= selectShapesofDCM(activepage,selection);
                List<string> failures = new List<string>();
                bool valid=CheckGoalSelection(activepage, pastedpage, selection,failures);
                if (valid)
                {
                    CheckUpperFeature(pastedpage);
                }
                else
                {
                    pastedpage.Delete(1);
                    //Throw exception with all found errors
                    string errors = "";
                    foreach (var e in failures)
                    {
                        errors += e;
                        errors += Environment.NewLine;
                    }
                    if (failures.Count != 0)
                        System.Windows.Forms.MessageBox.Show(errors, "Generation failed!");
                }
                
            }
        }
        private void CheckUpperFeature(IVPage pastedpage)
        {
            foreach (var st in pastedpage.Shapes)
            {
                if (st.Name.Contains("System Type"))
                {
                    if (st.ConnectedShapes(VisConnectedShapesFlags.visConnectedShapesIncomingNodes,"").Count()>=1 && st.ConnectedShapes(VisConnectedShapesFlags.visConnectedShapesOutgoingNodes, "").Count() >= 2)
                    {
                        IVShape inlink = getInlink(st.Name, pastedpage);
                        int minlink = 0;
                        int maxlink = 0;

                        if (!inlink.Text.Equals("") && !inlink.Text.Equals("[0...0]"))
                        {
                            string[] invalues = inlink.Text.Split('[');
                            string[] invalues2 = invalues[1].Split(']');
                            string[] inminandmax = invalues2[0].Split('.');
                            Int32.TryParse(inminandmax[0], out minlink);
                            Int32.TryParse(inminandmax[3], out maxlink);
                        }
                        List<IVShape> outlinks = getoutlinks(st.Name, pastedpage);
                        int[,] valueadd = new int[2, outlinks.Count];
                        int[,] valueor = new int[2, outlinks.Count];
                        int counter = 0;
                        foreach (var li in outlinks)
                        {
                            
                            int min = 0;
                            int max = 0;
                            if (!li.Text.Equals("") && !inlink.Text.Equals("[0...0]"))
                            {

                            
                            string[] values = li.Text.Split('[');
                            string[] values2 = values[1].Split(']');
                            string[] minandmax = values2[0].Split('.');
                            Int32.TryParse(minandmax[0], out min);
                            Int32.TryParse(minandmax[3], out max);
                            if (li.Text.Contains("OR"))
                            {
                                valueor[0, counter] = min;
                                valueor[1, counter] = max;
                            }
                            else
                            {
                                valueadd[0, counter] = min;
                                valueadd[1, counter] = max;
                            }
                            counter++;
                            }
                        }
                        int minor = getminimum(valueor);
                        int maxor = getSum(valueor, false);
                        int minand = getSum(valueadd,true);
                        int maxand = getSum(valueadd,false);
                        if (minand >= minlink) minlink = minand;
                        if (((maxor + maxand) <= maxlink) || maxlink == 0)
                        {
                            if (!unendlich(valueadd))maxlink = maxor + maxand;
                        }
                        
                       

                        if (inlink.Text.Contains("OR"))
                        {
                            if (maxlink != 0) inlink.Text = "OR[" + minlink + "..." + maxlink + "]";
                            else inlink.Text = "OR[" + minlink + "..." + "n]";
                        }

                        else if (inlink.Text.Contains("AND"))
                        {
                            if (maxlink != 0) inlink.Text = "AND[" + minlink + "..." + maxlink + "]";
                            else inlink.Text = "AND[" + minlink + "..." + "n]";
                        }

                        else
                        {
                            if (maxlink != 0) inlink.Text = "[" + minlink + "..." + maxlink + "]";
                            else inlink.Text = "[" + minlink + "..." + "n]";
                        }

                    }
                }
            }
        }

        private bool unendlich(int[,] valueadd)
        {
            bool unendlich = false;

            for (int i = 0; i < valueadd.GetLength(1); i++)
            {
                
                if (valueadd[1, i] == 0)
                    unendlich = true;
               
                if (valueadd[0, i] == 0)
                    unendlich = true;
               
                
            }
            return unendlich;
        }

        private int getSum(int[,] values, bool v)
        {
            int sum = 0;

            for (int i = 0; i < values.GetLength(1); i++)
            {
                
                if (v)
                {
                    sum += values[0, i];
                }
                else
                {
                    sum += values[1, i];
                }
            }
            return sum;
        }
        private int getminimum(int[,] valueor)
        {
            int min = 0;
            
            for (int i = 0; i < valueor.GetLength(1); i++)
            {
                if (min == 0) min = valueor[0, i];
                if (valueor[0, i] <= min && valueor[0, i] != 0)
                {
                    min = valueor[0, i];
                }
            }
            return min;
        }
        private List<IVShape> getoutlinks(string name, IVPage pastedpage)
        {
            List<IVShape> outlinks = new List<IVShape>();
            foreach (var l in pastedpage.Shapes)
            {
                if (l.Name.Contains("Link"))
                {
                    if (l.Cells("BegTrigger").FormulaU.Contains(name + "'"))
                    {
                        outlinks.Add(l);
                    }
                }
            }
            return outlinks;
        }
        private IVShape getInlink(string name, IVPage pastedpage)
        {
            IVShape inlink = new IVShape();
            foreach (var l in pastedpage.Shapes)
            {
                if (l.Name.Contains("Link"))
                {
                    if (l.Cells("EndTrigger").FormulaU.Contains(name + "'"))
                    {
                        inlink = l;
                    }
                }
            }
            return inlink;
        }
        private void deleteshapes(List<IVShape> unreachablegoals, IVPage pastedpage)
        {
            foreach (var s in unreachablegoals)
            {
                //first delete unreachable goal
              string stext=  s.Text;
                deleteshape(s, pastedpage);
                // durchlaufe solange das Modell, bis keine leere And/Or Nodes mehr vorhanden sind
                bool exists = checkforemptynodes(pastedpage); ;
                while (exists)
                {
                    IVShape emptyNode = getemptynode(pastedpage);
                    List<IVShape> decompositions = getdecompositions(emptyNode, pastedpage);
                    //durchlaufe Liste der Dekompositionen und hole dazugehörige Ziele, Tasks, etc.
                    List<IVShape> connectedShapes = getTargets(decompositions, pastedpage);
                    //loeschen der Node und dazugehöriger LInks+ TargetShapes
                     foreach (var d in decompositions)
                    {
                        
                        string name = d.Name;
                        string textd = d.Text;
                        d.Delete();
                        //deleteshape(d, pastedpage);
                    }
                    emptyNode.Delete();
                    foreach (var cs in connectedShapes)
                    {
                        string deletedshape = cs.Text;
                        cs.Delete();
                    }

                    //Pruefe, ob weitere Nodes existieren
                    exists = checkforemptynodes(pastedpage);
                }
            }
            //suche nicht verbundene Decomposition und Contribution Links und entferne diese von pastedpage
            List <IVShape> list=getemptylinks(pastedpage);
            foreach (var l in list)
            {
                l.Delete();
            }

        }
        private List<IVShape> getTargets(List<IVShape> decompositions, IVPage pastedpage)
        {
            List<IVShape> elements = new List<IVShape>();
            foreach (var d in decompositions)
            {
                foreach (var s in pastedpage.Shapes)
                {
                    if (d.Cells("EndTrigger").FormulaU.Contains(s.Name+ "!"))
                    {
                        elements.Add(s);
                    }
                }
            }
            return elements;
        }
        private List<IVShape> getdecompositions(IVShape emptyNode, IVPage pastedpage)
        {
            List<IVShape> links = new List<IVShape>();
            foreach (var s in pastedpage.Shapes)
            {
                if (s.Cells("BegTrigger").FormulaU.Contains(emptyNode.Name))
                    links.Add(s);
            }
            return links;
        }
        private bool checkforemptynodes(IVPage pastedpage)
        {
            bool exists = false;
            foreach (var s in pastedpage.Shapes)
            {
                if (s.Name.Contains("IOR-Node")|| s.Name.Contains("XOR-Node") || s.Name.Contains("AND-Node"))
                {
                    if (!(s.Cells("EndTrigger").FormulaU.Contains("Goal")||
                        s.Cells("EndTrigger").FormulaU.Contains("Softgoal") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Task") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Resource") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Indicator") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Belief")) &&
                        !(s.Cells("BegTrigger").FormulaU.Contains("Goal") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Softgoal") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Task") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Resource") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Indicator") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Belief")))
                    {
                        exists = true;
                    }
                }
            }
            return exists;
        }

        private List<IVShape> getemptylinks(IVPage pastedpage)
        {
            List <IVShape> emptylinks = new List<IVShape>();
            foreach (var s in pastedpage.Shapes)
            {
                if (s.Name.Contains("Contribution"))
                {
                    if (!(s.Cells("EndTrigger").FormulaU.Contains("Goal") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Softgoal") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Task") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Resource") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Indicator") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Belief")) ||
                        !(s.Cells("BegTrigger").FormulaU.Contains("Goal") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Softgoal") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Task") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Resource") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Indicator") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Belief")))
                    {
                        emptylinks.Add(s);
                    }

                }
                if (s.Name.Contains("Decomposition"))
                {
                    if (!(s.Cells("EndTrigger").FormulaU.Contains("Goal") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Softgoal") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Task") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Resource") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Indicator") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Belief")))
                    {
                        emptylinks.Add(s);
                    }
                }
            }
            return emptylinks;
        }
        private IVShape getemptynode(IVPage pastedpage)
        {
            IVShape emptynode = new IVShape();
            foreach (var s in pastedpage.Shapes)
            {
                if (s.Name.Contains("IOR-Node") || s.Name.Contains("XOR-Node") || s.Name.Contains("AND-Node"))
                {
                    if (!(s.Cells("EndTrigger").FormulaU.Contains("Goal") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Softgoal") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Task") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Resource") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Indicator") ||
                        s.Cells("EndTrigger").FormulaU.Contains("Belief")) &&
                        !(s.Cells("BegTrigger").FormulaU.Contains("Goal") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Softgoal") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Task") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Resource") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Indicator") ||
                        s.Cells("BegTrigger").FormulaU.Contains("Belief")))
                    {
                        emptynode = s;
                    }
                }
            }
            return emptynode;
        }
        private void deleteshape(IVShape s, IVPage pastedpage)
        {
            foreach (var shape in pastedpage.Shapes)
            {
                if (shape.Name.Equals(s.Name))
                    shape.Delete();
            }

        }
        private List<IVShape> CheckConfiguration(IVPage activepage, IVPage pastedpage, string[,] values)
        {
            //ueberpruefe requires und excludes Links basierend auf eingetragener KOnfiguration
            // bei requires müssen auch alle or und and Erweiterungen betrachtet werden
            //wenn Ziel nicht erfüllbar, speichern in Liste und Unterelemente ermitteln und ebenfalls in Liste abspeichern
            //Listenelemente aus pastedpage entfernen 
            List<IVShape> unreachablegoals = new List<IVShape>();
            bool invalid = false;
            foreach (var s in activepage.Shapes)
            {
                if (s.Name.Contains("Excludes"))
                {
                    // wenn der Wert des Features >=1 ist, dann kann das Ziel nicht erfüllt werden
                    IVShape target = getTargetShape(s,activepage);
                    IVShape source = getSourceShape(s, activepage);
                    int i=getIndexof(values, target.Text);
                    int value = Int32.Parse(values[1, i]);
                    if (value >= 1)
                    {
                        unreachablegoals.Add(source);
                    }
                }
                else if (s.Name.Contains("Requires Link"))
                {
                    IVShape target = getSourceShape(s, activepage);
                    IVShape source = getTargetShape(s, activepage);
                    List<IVShape> OrRequires = getOrRequires(s, activepage);
                    List<IVShape> AndRequires = getAndrequires(s, activepage);
                    bool valid = true;
                    //beachten: Source verweist auf Feature
                    if (OrRequires.Count==0 && AndRequires.Count == 0)
                    {
                        int i = getIndexof(values, target.Text);
                        int value = Int32.Parse(values[1, i]);
                        string minandmax = getMinandMax(s);
                        string[] split = minandmax.Split(',');
                        string min = split[0]; 
                        string max = split[1];
                        int mini=0;
                        int maxi=0;
                        Int32.TryParse(min, out mini);
                        Int32.TryParse(max, out maxi);
                        if (maxi == 0)
                        {
                            if (!(mini <= value) )
                            {
                                valid = false;
                            }
                        }
                        else
                        {
                            if (!(mini <= value)  || !(value<= maxi))
                            {
                                valid = false;
                            }
                        }



                    }
                    else if (OrRequires.Count != 0 && AndRequires.Count == 0)
                    {
                        //durchlaufe Shapes aus Liste und pruefe, ob mind. eine Bedingung erfuellt ist
                        bool orvalid = false;
                        int i = getIndexof(values, target.Text);
                        int value = Int32.Parse(values[1, i]);
                        string minandmax = getMinandMax(s);
                        string[] split = minandmax.Split(',');
                        string min = split[0];
                        string max = split[1];
                        int mini = 0;
                        int maxi = 0;
                        Int32.TryParse(min, out mini);
                        Int32.TryParse(max, out maxi);
                        if (maxi == 0)
                        {
                            if ((mini <= value))
                            {
                                orvalid = true;
                            }
                        }
                        else
                        {
                            if ((mini <= value) && (value <= maxi))
                            {
                                orvalid = true;
                            }
                        }
                        foreach (IVShape orr in OrRequires)
                        {
                            IVShape targetorr = getSourceShape(orr, activepage);
                            int ior = getIndexof(values, targetorr.Text);
                            int valueor = Int32.Parse(values[1, ior]);
                            string minandmaxor = getMinandMax(orr);
                            string[] splitor = minandmaxor.Split(',');
                            string minor = splitor[0];
                            string maxor = splitor[1];
                            int minior = 0;
                            int maxior = 0;
                            Int32.TryParse(minor, out minior);
                            Int32.TryParse(maxor, out maxior);
                            if (maxior == 0)
                            {
                                if ((minior <= valueor))
                                {
                                    orvalid = true;
                                }
                            }
                            else
                            {
                                if ((minior <= valueor) && (valueor <= maxior))
                                {
                                    orvalid = true;
                                }
                            }
                        }
                        if (!orvalid)
                        {
                            valid = false;
                        }
                    }
                    else if (OrRequires.Count == 0 && AndRequires.Count != 0)
                    {
                        //durchlaufe Shapes aus Liste und pruefe, ob alle Bedingungen erfuellt sind
                        bool andvalid = true;
                        int i = getIndexof(values, target.Text);
                        int value = Int32.Parse(values[1, i]);
                        string minandmax = getMinandMax(s);
                        string[] split = minandmax.Split(',');
                        string min = split[0];
                        string max = split[1];
                        int mini = 0;
                        int maxi = 0;
                        Int32.TryParse(min, out mini);
                        Int32.TryParse(max, out maxi);
                        if (maxi == 0)
                        {
                            if (!(mini <= value))
                            {
                                andvalid = false;
                            }
                        }
                        else
                        {
                            if (!(mini <= value) || !(value <= maxi))
                            {
                                andvalid = false;
                            }
                        }
                        foreach (IVShape and in AndRequires)
                        {
                            IVShape targetand = getSourceShape(and, activepage);
                            int iand = getIndexof(values, targetand.Text);
                            int valueand = Int32.Parse(values[1, iand]);
                            string minandmaxand = getMinandMax(and);
                            string[] splitand = minandmax.Split(',');
                            string minand = split[0];
                            string maxand = split[1];
                            int miniand = 0;
                            int maxiand = 0;
                            Int32.TryParse(minand, out miniand);
                            Int32.TryParse(maxand, out maxiand);
                            if (maxiand == 0)
                            {
                                if (!(miniand <= valueand))
                                {
                                    andvalid = false;
                                }
                            }
                            else
                            {
                                if (!(miniand <= valueand) || !(valueand <= maxiand))
                                {
                                    andvalid = false;
                                }
                            }
                        }
                        if (!andvalid)
                        {
                            valid = false;
                        }
                    }
                    else if (OrRequires.Count != 0 && AndRequires.Count != 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Requires Links include OR and AND Addition, which is not valid. " +
                            "Please change this error and run the function again to create the view.");
                        pastedpage.Delete(1);
                        invalid = true;
                    }
                        if (!valid)
                    {
                        unreachablegoals.Add(source);
                    }
                }
            }
            if (invalid)
            {
                unreachablegoals = new List<IVShape>();
            }
            return unreachablegoals;
        }
        private string getMinandMax(IVShape s)
        {
            //Durchlaufe Subshapes und speichere Werte als String getrennt durch Komma;
            string values = "";
            string helper = "";
            foreach (var sub in s.Shapes)
            {
                if (sub.Name.Contains("Cardinality"))
                {
                    helper = sub.Text;
                }
            }
            string[] minmax= helper.Split(new string[] { "..." }, StringSplitOptions.RemoveEmptyEntries);
            return values = minmax[0].Substring(1) + ","+ minmax[1].Remove(1);
        }
        private int getIndexof(string[,] values, string text)
        {
            int i = 0;
            for (int j = 0; j < values.GetLength(1); j++)
            {
                if (text.Equals(values[0, j]))
                {
                    i = j;
                }
            }
            return i;
        }
        private IVShape getSourceShape(IVShape s, IVPage activePage)
        {
            IVShape source = new IVShape();
            string frm = s.Cells("BegTrigger").FormulaU;
            string helper = frm.Remove(frm.Length - 12);
            string fromname = helper.Substring(11);
            foreach (var src in activePage.Shapes)
            {
                if (fromname.Contains(src.Name))
                {
                    source = src;
                }
            } 
            return source;
        }
        private IVShape getTargetShape(IVShape s, IVPage activePage)
        {
            IVShape target=new IVShape();
            
            string to = s.Cells("EndTrigger").FormulaU;
            string helper = to.Remove(to.Length - 12);
            string toname = helper.Substring(11);
            foreach (var trgt in activePage.Shapes)
            {
                if (toname.Contains(trgt.Name))
                {
                    target = trgt;
                }
            }
            return target;
        }
        private List<IVShape> getAndrequires(IVShape s, IVPage activePage)
        {
            //beachten: Target und Source sind vertauscht
            List<IVShape> ANDRequires = new List<IVShape>();
            foreach (var ands in activePage.Shapes)
            {
                if (ands.Name.Contains("AND Requires"))
                {
                    if (ands.Cells("EndTrigger").FormulaU.Contains(s.Name))
                    {
                        ANDRequires.Add(ands);
                    }

                }
            }
            return ANDRequires;
        }
        private List<IVShape> getOrRequires(IVShape s, IVPage activePage)
        {
            //beachten: Target und Source sind vertauscht
            List<IVShape> ORRequires = new List<IVShape>();
            foreach (var ors in activePage.Shapes)
            {
                if(ors.Name.Contains("OR Requires"))
                {
                    if (ors.Cells("EndTrigger").FormulaU.Contains(s.Name))
                    {
                        ORRequires.Add(ors);
                    }
                        
                }
            }

            return ORRequires;
        }
        private bool CheckGoalSelection(IVPage activepage, IVPage pastedpage,IVSelection Selection,List<String> failures)
        {
            bool validselection = true;
            foreach (var goal in Selection)
            {
                foreach (var s in activepage.Shapes)
                {
                    if (goal.Name.Equals(s.Name))
                    {
                        //ermittle verbundene excludes und requires Links
                        List <IVShape> excludes= getexcludesLink(s,activepage);
                        List <IVShape> requires = getrequiresLink(s, activepage);
                        foreach (var ex in excludes)
                        {
                            //hole System Type (Endtrigger of excludes link)
                            IVShape systemtype = getTargetShape(ex, activepage);
                            // hole dazugehörigen Link und setze Feld wenn nicht leer auf "[0...0]"

                            //wenn Feld nicht leer, prüfen ob es sich um ein optionales Feld handelt oder um ein And Requires
                            //bei optional--> "[0...0]" ansonsten Fehlermeldung (bool auf false setzen)
                            foreach (var l in pastedpage.Shapes)
                            {
                                if (l.Name.Contains("Link"))
                                {
                                    if (l.Cells("EndTrigger").FormulaU.Contains(systemtype.Name + "'"))
                                    {
                                        if (l.Text.Equals("") || l.Text.Contains("OR"))
                                        {
                                            l.Text = "[0...0]";
                                        
                                        }
                                        else if (l.Text.Contains("AND") || (l.Text.StartsWith("[") && !(l.Text.Equals("[0...0]"))))
                                        {
                                            failures.Add("No possible Configuration available regarding to System Type " + systemtype.Text);
                                            validselection = false;
                                        }
                                    }
                                }
                            }

                        }
                        foreach (var req in requires)
                        {
                            //hole System Type (Begtrigger of requires link)
                            IVShape systemtype = getSourceShape(req, activepage);
                            List<IVShape> OrRequires = getOrRequires(req, activepage);
                            List<IVShape> AndRequires = getAndrequires(req, activepage);
                            foreach (var l in pastedpage.Shapes)
                            {
                                if (l.Name.Contains("Link"))
                                {
                                    string name = l.Cells("EndTrigger").FormulaU;
                                    if (l.Cells("EndTrigger").FormulaU.Contains(systemtype.Name + "'"))
                                    {
                                        if(OrRequires.Count==0 && AndRequires.Count == 0)
                                        {
                                            if (l.Text.Equals("") || l.Text.Contains("OR"))
                                            {
                                                l.Text = getValueofLink(req);
                                            }
                                            
                                            //Fall mit Min und Max anpassen, falls schon Werte gesetzt sind
                                            else if (l.Text.Contains("[")&&!l.Text.Equals("[0...0]"))
                                            {
                                                String newvalues = checkvalues(getValueofLink(req), l.Text, true);
                                                if (newvalues.Contains("error"))
                                                {
                                                    failures.Add("No possible Configuration available regarding to System Type " + systemtype.Text);
                                                    validselection = false;
                                                }
                                                else
                                                {
                                                    if (newvalues.Contains("AND"))
                                                    {
                                                        string [] helper = newvalues.Split('D');
                                                        l.Text = helper[1];
                                                    }
                                                    else
                                                    {
                                                        l.Text = newvalues;
                                                    }
                                                }
                                            }

                                            else if (l.Text.Equals("[0...0]"))
                                            {
                                                failures.Add("No possible Configuration available regarding to System Type " + systemtype.Text);
                                                validselection = false;
                                            }
                                        }
                                        else if (OrRequires.Count != 0 && AndRequires.Count == 0)
                                        {
                                            if (l.Text.Equals(""))
                                            {
                                                l.Text = "OR" + getValueofLink(req);
                                            }
                                            else if ((l.Text.Contains("OR") || (l.Text.StartsWith("["))) && !l.Text.Equals("[0...0]"))
                                            {
                                                String newvalues = checkvalues(getValueofLink(req), l.Text, false);
                                                if (newvalues.Contains("error"))
                                                {
                                                    failures.Add("No possible Configuration available regarding to System Type " + systemtype.Text);
                                                    validselection = false;
                                                }
                                                else
                                                {
                                                    l.Text = "OR"+newvalues;
                                                }
                                            }
                                            else if (l.Text.Equals("[0...0]"))
                                            {
                                                bool check = checkorrequires(activepage, pastedpage, OrRequires, req);
                                                if (check)
                                                {
                                                    failures.Add("No possible Configuration available regarding to System Type " + systemtype.Text);
                                                    validselection = false;
                                                }
                                                
                                            }
                                        }
                                        else if (OrRequires.Count == 0 && AndRequires.Count != 0)
                                        {
                                            if (l.Text.Equals("") || l.Text.Contains("OR"))
                                            {
                                                l.Text = "AND" + getValueofLink(req);
                                            }
                                            else if (l.Text.Contains("AND") || l.Text.StartsWith("["))
                                            {
                                                String newvalues = checkvalues(getValueofLink(req), l.Text, true);
                                                if (newvalues.Contains("error"))
                                                {
                                                    failures.Add("No possible Configuration available regarding to System Type " + systemtype.Text);
                                                    validselection = false;
                                                }
                                                else
                                                {
                                                    l.Text = newvalues;
                                                }
                                            }
                                                
                                            else if(l.Text.Equals("[0...0]"))
                                            {
                                                failures.Add("No possible Configuration available regarding to System Type " + systemtype.Text);
                                                validselection = false;
                                            }
                                        }
                                        else if (OrRequires.Count != 0 && AndRequires.Count != 0)
                                        {
                                            failures.Add("No possible Configuration available regarding to Requires Additions. " +
                                                "Requires Links include OR and AND Addition, which is not valid.");
                                            validselection = false;
                                        }

                                    }
                                }
                            }
                            //durchlaufe alle OR und AND Requires und passe Werte an
                            foreach (var or in OrRequires)
                            {
                                IVShape systemtyp = getSourceShape(or, activepage);
                                foreach (var l in pastedpage.Shapes)
                                {
                                    if (l.Name.Contains("Link"))
                                    {
                                        if (l.Cells("EndTrigger").FormulaU.Contains(systemtyp.Name + "'"))
                                        {
                                           if (OrRequires.Count != 0 && AndRequires.Count == 0)
                                            {
                                                if (l.Text.Equals(""))
                                                {
                                                    l.Text = "OR" + getValueofLink(or);
                                                }
                                                else if (l.Text.Contains("OR") || (l.Text.StartsWith("[")))
                                                {
                                                    String newvalues = checkvalues(getValueofLink(or), l.Text, false);
                                                    if (newvalues.Contains("error"))
                                                    {
                                                        failures.Add("No possible Configuration available regarding to System Type " + systemtype.Text);
                                                        validselection = false;
                                                    }
                                                    else
                                                    {
                                                        l.Text = "OR" + newvalues;
                                                    }
                                                }
                                                else if (l.Text.Equals("[0...0]"))
                                                {
                                                    bool check = checkorrequires(activepage, pastedpage, OrRequires, req);
                                                    if (check)
                                                    {
                                                        failures.Add("No possible Configuration available regarding to System Type " + systemtype.Text);
                                                        validselection = false;
                                                    }

                                                }
                                            }                                       
                                        }
                                    }
                                }
                            }
                            foreach (var and in AndRequires)
                            {
                                IVShape systemtyp = getSourceShape(and, activepage);
                                foreach (var l in pastedpage.Shapes)
                                {
                                    if (l.Name.Contains("Link"))
                                    {
                                        if (l.Cells("EndTrigger").FormulaU.Contains(systemtyp.Name + "'"))
                                        {
                                            if (OrRequires.Count == 0 && AndRequires.Count != 0)
                                            {
                                                if (l.Text.Equals("") || l.Text.Contains("OR"))
                                                {
                                                    l.Text = "AND" + getValueofLink(and);
                                                }
                                                else if ((l.Text.Contains("AND") || l.Text.StartsWith("[")) && !l.Text.Equals("[0...0]"))
                                                {
                                                    String newvalues = checkvalues(getValueofLink(and), l.Text, true);
                                                    if (newvalues.Contains("error"))
                                                    {
                                                        failures.Add("No possible Configuration available regarding to System Type " + systemtype.Text);
                                                        validselection = false;
                                                    }
                                                    else
                                                    {
                                                        l.Text = newvalues;
                                                    }
                                                }

                                                else if (l.Text.Equals("[0...0]"))
                                                {
                                                    failures.Add("No possible Configuration available regarding to System Type " + systemtype.Text);
                                                    validselection = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return validselection;
        }
        private bool checkorrequires(IVPage activepage, IVPage pastedpage, List<IVShape> orRequires, IVShape req)
        {
            bool exists = false;
            foreach (var or in orRequires)
            {
                foreach (var st in pastedpage.Shapes)
                {
                    if (or.Cells("BegTrigger").FormulaU.Contains(st.Name + "'") || req.Cells("BegTrigger").FormulaU.Contains(st.Name + "'"))
                    if (!valueofsystemtype(st.Name + "'", pastedpage).Contains("[0...0]"))
                    {
                        exists = true;
                    }
                        
                }
            }

            return exists;
        }
        private string valueofsystemtype(String name, IVPage page)
        {
            string value = "";
            foreach (var s in page.Shapes)
            {
                if (s.Name.Contains("Link"))
                {
                   if (s.Cells("EndTrigger").FormulaU.Contains(name))
                    {
                        value = s.Text;
                    }
                }
            }
            return value;
        }
        private string checkvalues(string v, string text, bool and)
        {
            string newvalues = "";
            int mina = 0;
            int maxa = 0;
            int minb = 0;
            int maxb = 0;
            string[] avalues = v.Split('[');
            string[] avalues2 = avalues[1].Split(']');
            string[] aminandmax = avalues2[0].Split('.');
            string[] bvalues = text.Split('[');
            string[] bvalues2 = bvalues[1].Split(']');
            string[] bminandmax = bvalues2[0].Split('.');
            Int32.TryParse(aminandmax[0], out mina);
            Int32.TryParse(aminandmax[3], out maxa);
            Int32.TryParse(bminandmax[0], out minb);
            Int32.TryParse(bminandmax[3], out maxb);
            if (mina==minb && maxa == maxb)
            {
                if (and)
                {
                    if (maxa==0)
                    newvalues = "AND[" + mina + "..."  + "M]";
                    else newvalues = "AND[" + mina + "..." + maxa + "]";
                }

                else
                {
                    if (maxa == 0)
                    {
                        newvalues = "[" + mina + "..."  + "M]";
                    }
                    else newvalues = "[" + mina + "..." + maxa + "]";
                }
                    
            }
            else if (mina == minb && maxa < maxb)
            {
                {

                    if (and)
                    {
                        if (maxa == 0)newvalues = "AND[" + mina + "..." +  maxb +"]";
                        else  newvalues = "AND[" + mina + "..." + maxa + "]";
                    }

                    else
                    {
                        if (maxa == 0)newvalues = "[" + mina + "..." +  maxb+ "]";
                        else newvalues = "[" + mina + "..." + maxa + "]";
                    }

                }
            }
            else if (mina == minb && maxa > maxb)
            {
                {
                    if (and)
                    {
                        if (maxb == 0) newvalues = "AND[" + mina + "..." + maxa+ "]";
                        else newvalues = "AND[" + mina + "..." + maxb + "]";
                    }

                    else
                    {
                        if (maxb == 0)newvalues = "[" + mina + "..." +  maxa+"]";
                        else newvalues = "[" + mina + "..." + maxb + "]";
                    }

                }
            }
            else if (mina < minb && maxa == maxb) 
            {
                     if (and)
                    {
                        if (maxb == 0) newvalues = "AND[" + minb + "..." + "M]";
                        else newvalues = "AND[" + minb + "..." + maxa + "]";
                    }
                    else
                    {
                        if (maxb == 0) newvalues = "[" + minb + "..." + "M]";
                        else newvalues = "[" + minb + "..." + maxa + "]";
                    }
            }
            else if (mina < minb && maxa < maxb)
            {
                
                    if (and)
                    {
                        if (maxa == 0) newvalues = "AND[" + minb + "..." + maxb + "]";
                        else if (minb <= maxa)newvalues = "AND[" + minb + "..." + maxa + "]";
                        else newvalues = "error";    
                    }
                    else
                    {
                        if (maxa == 0) newvalues = "[" + minb + "..." + maxb + "]";
                        else if (minb <= maxa) newvalues = "[" + minb + "..." + maxa + "]";
                        else newvalues = "error";
                    }

                
            }
            else if (mina < minb && maxa > maxb)
            {
                if (and)
                {
                    if (maxb == 0) newvalues = "AND[" + minb + "..." + maxa + "]";
                    else if (mina <= maxb) newvalues = "AND[" + minb + "..." + maxb + "]";
                    else newvalues = "error";
                }
                else
                {
                    if (maxb == 0) newvalues = "[" + minb + "..." + maxa + "]";
                    else if (mina <= maxb) newvalues = "[" + minb + "..." + maxb + "]";
                    else newvalues = "error";
                }
            }
            else if (mina > minb && maxa == maxb)
            {
                if (and)
                {
                    if (maxb == 0) newvalues = "AND[" + mina + "..." + "M]";
                    else newvalues = "AND[" + mina + "..." + maxa + "]";
                }
                else
                {
                    if (maxb == 0) newvalues = "[" + mina + "..." + "M]";
                    else newvalues = "[" + mina + "..." + maxa + "]";
                }
            }
            else if (mina > minb && maxa < maxb)
            {
                if (and)
                {
                    if (maxa == 0) newvalues = "AND[" + mina + "..." + maxb + "]";
                    else if (mina <= maxb) newvalues = "AND[" + mina + "..." + maxa + "]";
                    else newvalues = "error";
                }
                else
                {
                    if (maxa == 0) newvalues = "[" + mina + "..." + maxb + "]";
                    else if (minb <= maxa) newvalues = "[" + mina + "..." + maxa + "]";
                    else newvalues = "error";
                }
            }
            else if (mina > minb && maxa > maxb)
            {
                if (and)
                {
                    if (maxb == 0) newvalues = "AND[" + mina + "..." + maxa + "]";
                    else if (mina <= maxb) newvalues = "AND[" + mina + "..." + maxb + "]";
                    else newvalues = "error";
                }
                else
                {
                    if (maxa == 0) newvalues = "[" + mina + "..." + maxa + "]";
                    else if (mina <= maxb) newvalues = "[" + mina + "..." + maxb + "]";
                    else newvalues = "error";
                }
            }
            return newvalues;
        }
        private string getValueofLink(IVShape req)
        {
            string value = "";
            foreach (var sub in req.Shapes)
            {
                if (sub.Name.Contains("Cardinality"))
                {
                    value = sub.Text;
                }
            }
            return value;
        }
        private List<IVShape> getrequiresLink(IVShape s, IVPage activepage)
        {
            List<IVShape> requires = new List<IVShape>();
            foreach (var link in activepage.Shapes)
            {
                if (link.Name.Contains("Requires Link"))
                {
                    if (link.Cells("EndTrigger").FormulaU.Contains(s.Name))
                    {
                        requires.Add(link);
                    }
                }
            }
            return requires;
        }
        private List<IVShape> getexcludesLink(IVShape s, IVPage activepage)
        {
            List<IVShape> excludes = new List<IVShape>();
            foreach (var link in activepage.Shapes)
            {
                if (link.Name.Contains("Excludes"))
                {
                    if (link.Cells("BegTrigger").FormulaU.Contains(s.Name))
                    {
                        excludes.Add(link);
                    }
                }
            }
            return excludes;
        }
        private IVPage selectShapesofDCM(IVPage activePage, IVSelection selection)
        {
            IVPage pastedpage = activePage.Duplicate();
            pastedpage.Name = "Configuration View_" + DateTime.Now;
            List<IVShape> shapestodelete = new List<IVShape>();
            foreach (var s in pastedpage.Shapes)
            {
                
                if (!(s.Name.Contains("System Type")||
                    s.Name.Contains("Multiple Cardinality") ||
                    s.Name.Contains("Physical Connection") ||
                    s.Name.Contains("OR Cardinality") ||
                    s.Name.Contains("Link with Cardinality")))
                {
                    if (!s.Name.Contains("Link"))
                        shapestodelete.Add(s);
                    else
                    {
                        if (!s.Name.Substring(0, 4).Equals("Link"))
                            shapestodelete.Add(s);
                    }
                    
                }

            }
            foreach (var s in shapestodelete)
            {
                s.Delete();
            }
            IVShape info = pastedpage.DrawRectangle(1, 1, 3.5, 3.5);

            info.SetCenter(2,12);

            string Text = "Selected Goals";

            foreach (IVShape s in selection)
            {
                Text += Environment.NewLine;
                Text += " " + s.Text;
            }
            info.Text = Text;
            return pastedpage;
        }
        private IVPage selectShapesofGRL(IVPage activePage, string[,] values)
        {
            IVPage pastedpage = activePage.Duplicate();
            pastedpage.Name = "Goal View_" + DateTime.Now; ;
            List<IVShape> shapestodelete = new List<IVShape>();
            foreach (var s in pastedpage.Shapes)
            {

                if (!(s.Name.Contains("Actor") ||
                    s.Name.Contains("Task") ||
                    s.Name.Contains("Belief") ||
                    s.Name.Contains("Resource") ||
                    s.Name.Contains("Goal") ||
                    s.Name.Contains("Softgoal") ||
                    s.Name.Contains("Indicator") ||
                    s.Name.Contains("Decomposition") ||
                    s.Name.Contains("Contribution Link") ||
                    s.Name.Contains("Correlation Link") ||
                    s.Name.Contains("Make") ||
                    s.Name.Contains("Help") ||
                    s.Name.Contains("Some Positive") ||
                    s.Name.Contains("Some Negative") ||
                    s.Name.Contains("Unknown") ||
                    s.Name.Contains("Break") ||
                    s.Name.Contains("Hurt") ||
                    s.Name.Contains("Dependency") ||
                    s.Name.Contains("Label:")))
                {
                    if (!s.Name.Contains("AND") || s.Name.Contains("OR"))
                        shapestodelete.Add(s);
                    else
                    {
                        if (s.Name.Contains("Cardinality")|| s.Name.Contains("Requires"))
                            shapestodelete.Add(s);
                    }

                }
            }
            foreach (var s in shapestodelete)
            {
                s.Delete();
            }
            IVShape info = pastedpage.DrawRectangle(1,1,3.5,3.5);
            info.SetCenter(2, 12);
            string Text = "Configuration";

            for (int j = 0; j < values.GetLength(1); j++)
            {
                Text += Environment.NewLine;
                Text += " " + values[0, j] + ": " + values[1, j];  
            }
            info.Text = Text;
            return pastedpage;
        }
        private Boolean ChecklinkSystem(int id)
        {
            Boolean islink = true;
            foreach (var s in this._application.ActivePage.Shapes)
            {
                if (s.ID==id)
                    if (s.Name.Contains("Requires")||s.Name.Contains("Physical") || s.Name.Contains("Excludes"))
                    {
                        islink = false;
                    }
            }
            return islink;
        }

        private void CreateTvp(string system, IVDocument doc)
        {

            foreach (Page page in doc.Pages)
            {
                page.Name = "TVP_" + system;

                IVShape header;
                IVShape boundary = page.DrawRectangle(0, 12, 9, 1); boundary.CellsSRC(1, 3, 1).FormulaU = "THEMEGUARD(RGB(0,0,0))";

                header = page.DrawRectangle(1, 1, 8, 1.5); header.Text = "Technical Viewpoint: " + system; header.SetCenter(4, (28 / 2.54));
                header.CellsSRC(1, 11, 4).Formula = "0"; header.LineStyle = "none"; header.CellsSRC(3, 0, 7).FormulaU = "24 pt";
                header.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            }
        }

        private void CreateLvp(string system, IVDocument doc)
        {
            foreach (Page page in doc.Pages)
            {
                page.Name = "LVP_" + system;

                IVShape header;
                IVShape boundary = page.DrawRectangle(0, 12, 9, 1); boundary.CellsSRC(1, 3, 1).FormulaU = "THEMEGUARD(RGB(0,0,0))";

                header = page.DrawRectangle(1, 1, 8, 1.5); header.Text = "Logical Viewpoint: " + system; header.SetCenter(4, (28 / 2.54));
                header.CellsSRC(1, 11, 4).Formula = "0"; header.LineStyle = "none"; header.CellsSRC(3, 0, 7).FormulaU = "24 pt";
                header.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            }
        }

        private void CreateFvp(string system, IVDocument doc)
        {
            foreach (Page page in doc.Pages)
            {
                page.Name = "FVP_" + system;

                IVShape header;
                IVShape boundary = page.DrawRectangle(0, 12, 9, 1); boundary.CellsSRC(1, 3, 1).FormulaU = "THEMEGUARD(RGB(0,0,0))";


                header = page.DrawRectangle(1, 1, 8, 1.5); header.Text = "Functional Viewpoint: " + system; header.SetCenter(4, (28 / 2.54));
                header.CellsSRC(1, 11, 4).Formula = "0"; header.LineStyle = "none"; header.CellsSRC(3, 0, 7).FormulaU = "24 pt";
                header.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";

            }
        }
        

        private void CreateRvp(string systemname, IVDocument doc)
        {
            //TODO neu erstellte Diagramme mit Shapesheet oeffnen
            IVPage page = new IVPage();
            IVShape header, kontext, neutral, bezogen;
            IVShape wissenskontext, funktKontext, struktKontext;
            IVShape goals, useMap, szenario;
            IVShape struktPerspektive, funktPerspektive, verhaltensPerspektive;
            IVHyperlink wkhl, skhl, fkhl, ghl, uchl, mschl, sphl, fphl, vphl;
            IVShape statusWk, statusfK, statussK, statusG, statusUcm, statusSz, statussP, statusfP, statusVp;
            foreach (Page p in doc.Pages)
            {
                page = p;
            }

            page.Name = "RVP_" + systemname;
            IVShape boundary = page.DrawRectangle(0, 12, 9, 1); boundary.CellsSRC(1, 3, 1).FormulaU = "THEMEGUARD(RGB(0,0,0))";
            header = page.DrawRectangle(1, 1, 8, 1.5); header.Text = "Requirements Viewpoint: " + systemname; header.SetCenter(4, (28 / 2.54));
            header.CellsSRC(1, 11, 4).Formula = "0"; header.LineStyle = "none"; header.CellsSRC(3, 0, 7).FormulaU = "24 pt";
            header.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            //Kontextmodelle
            kontext = page.DrawRectangle(1, 1, 3, 1.5); kontext.Text = "Context Models"; kontext.SetCenter((3 / 2.54), (25 / 2.54));
            kontext.CellsSRC(1, 11, 4).Formula = "0"; kontext.LineStyle = "none"; kontext.CellsSRC(3, 0, 7).FormulaU = "18 pt";
            kontext.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            wissenskontext = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); wissenskontext.Text = "Context of Knowledge";
            wissenskontext.SetCenter((3 / 2.54), (22 / 2.54)); wissenskontext.CellsSRC(1, 11, 4).Formula = "0";
            wissenskontext.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusWk = page.DrawOval(1, 1, 1.16, 1.16); statusWk.SetCenter(3 / 2.54, 20.5 / 2.54); statusWk.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
           
            struktKontext = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); struktKontext.Text = "Structural operational Context";
            struktKontext.SetCenter((8 / 2.54), (22 / 2.54)); struktKontext.CellsSRC(1, 11, 4).Formula = "0";
            struktKontext.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statussK = page.DrawOval(1, 1, 1.16, 1.16); statussK.SetCenter(8 / 2.54, 20.5 / 2.54); statussK.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
            funktKontext = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); funktKontext.Text = "Functional operational Context";
            funktKontext.SetCenter((13 / 2.54), (22 / 2.54)); funktKontext.CellsSRC(1, 11, 4).Formula = "0";
            funktKontext.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusfK = page.DrawOval(1, 1, 1.16, 1.16); statusfK.SetCenter(13 / 2.54, 20.5 / 2.54); statusfK.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

            //Loesungsneutrale Modelle
            neutral = page.DrawRectangle(1, 1, 4, 1.5); neutral.Text = "Solution-unaware Models"; neutral.SetCenter((4 / 2.54), (18 / 2.54));
            neutral.CellsSRC(1, 11, 4).Formula = "0"; neutral.LineStyle = "none"; neutral.CellsSRC(3, 0, 7).FormulaU = "18 pt";
            neutral.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            goals = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); goals.Text = "Goals";
            goals.SetCenter((3 / 2.54), (15 / 2.54)); goals.CellsSRC(1, 11, 4).Formula = "0";
            goals.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusG = page.DrawOval(1, 1, 1.16, 1.16); statusG.SetCenter(3 / 2.54, 13.5 / 2.54); statusG.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
            useMap = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); useMap.Text = "Use-Case Maps";
            useMap.SetCenter((8 / 2.54), (15 / 2.54)); useMap.CellsSRC(1, 11, 4).Formula = "0";
            useMap.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusUcm = page.DrawOval(1, 1, 1.16, 1.16); statusUcm.SetCenter(8 / 2.54, 13.5 / 2.54); statusUcm.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
            szenario = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); szenario.Text = "Scenarios";
            szenario.SetCenter((13 / 2.54), (15 / 2.54)); szenario.CellsSRC(1, 11, 4).Formula = "0";
            szenario.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusSz = page.DrawOval(1, 1, 1.16, 1.16); statusSz.SetCenter(13 / 2.54, 13.5 / 2.54); statusSz.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

            //Loesungsbezogene Modelle
            bezogen = page.DrawRectangle(1, 1, 4, 1.5); bezogen.Text = "Solution-oriented Models"; bezogen.SetCenter((4 / 2.54), (11 / 2.54));
            bezogen.CellsSRC(1, 11, 4).Formula = "0"; bezogen.LineStyle = "none"; bezogen.CellsSRC(3, 0, 7).FormulaU = "18 pt";
            bezogen.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            struktPerspektive = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); struktPerspektive.Text = "Structural Perspective";
            struktPerspektive.SetCenter((3 / 2.54), (8 / 2.54)); struktPerspektive.CellsSRC(1, 11, 4).Formula = "0";
            struktPerspektive.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statussP = page.DrawOval(1, 1, 1.16, 1.16); statussP.SetCenter(3 / 2.54, 6.5 / 2.54); statussP.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
            funktPerspektive = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); funktPerspektive.Text = "Function Perspective";
            funktPerspektive.SetCenter((8 / 2.54), (8 / 2.54)); funktPerspektive.CellsSRC(1, 11, 4).Formula = "0";
            funktPerspektive.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusfP = page.DrawOval(1, 1, 1.16, 1.16); statusfP.SetCenter(8 / 2.54, 6.5 / 2.54); statusfP.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
            verhaltensPerspektive = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); verhaltensPerspektive.Text = "Behavioral Perspective";
            verhaltensPerspektive.SetCenter((13 / 2.54), (8 / 2.54)); verhaltensPerspektive.CellsSRC(1, 11, 4).Formula = "0";
            verhaltensPerspektive.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusVp = page.DrawOval(1, 1, 1.16, 1.16); statusVp.SetCenter(13 / 2.54, 6.5 / 2.54); statusVp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

            //Weise erstellte Objekte zu
            wkhl=wissenskontext.AddHyperlink();
            wkhl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_CoK.vsdx"));
            skhl= struktKontext.AddHyperlink();
            skhl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_soC.vsdx"));
            fkhl= funktKontext.AddHyperlink();
            fkhl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_foC.vsdx"));

            ghl=goals.AddHyperlink();
            ghl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_Goals.vsdx"));
            uchl=useMap.AddHyperlink();
            uchl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_UCM.vsdx"));
            mschl= szenario.AddHyperlink();
            mschl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_MSC.vsdx"));

            sphl= struktPerspektive.AddHyperlink();
            sphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_stP.vsdx"));
            fphl = funktPerspektive.AddHyperlink();
            fphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_fuP.vsdx"));
            vphl = verhaltensPerspektive.AddHyperlink();
            vphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_BP.vsdx"));

        }

        private void CreateemptyModels(Application subapp, string path, string systemname, SPES_DocumentReferencer pReferencer)
        {
            var doct = subapp.Documents.Add("");
            //subapp.Documents.OpenEx("SMT_CoK.vssx", 4);
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_CoK.vsdx"));
            doct.Close();
            pReferencer.AddAssignment(systemname + "_RVP_CoK.vsdx", typeof(WissenskontextNetwork).Name);

            doct = subapp.Documents.Add("");
            //subapp.Documents.OpenEx("SMT_FuC.vssx", 4); 
            subapp.ActivePage.Name = "funktional Perspective";
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_foC.vsdx"));
            doct.Close();
            pReferencer.AddAssignment(systemname + "_RVP_foC.vsdx", typeof(FunktionellerKontextNetwork).Name);

            doct = subapp.Documents.Add("");
            //subapp.Documents.OpenEx("SMT_SoC.vssx", 4); 
            subapp.ActivePage.Name = "static Perspective";
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_soC.vsdx"));
            doct.Close();
            pReferencer.AddAssignment(systemname + "_RVP_soC.vsdx", typeof(StrukturellerKontextNetwork).Name);

            //Dokumente für Loesungsneutrale Modelle
            doct = subapp.Documents.Add("");
            //subapp.Documents.OpenEx("SMT_GRL.vssx", 4);
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_Goals.vsdx"));
            doct.Close();
            pReferencer.AddAssignment(systemname + "_RVP_Goals.vsdx", typeof(ZielmodellNetwork).Name);

            doct = subapp.Documents.Add("");
            //subapp.Documents.OpenEx("SMT_UCM.vssx", 4);
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_UCM.vsdx"));
            doct.Close();
            pReferencer.AddAssignment(systemname + "_RVP_UCM.vsdx", typeof(SzenarioUseCasesNetwork).Name);

            doct = subapp.Documents.Add("");
            //subapp.Documents.OpenEx("SMT_hMSC.vssx", 4); 
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_MSC.vsdx"));
            doct.Close();
            pReferencer.AddAssignment(systemname + "_RVP_MSC.vsdx", typeof(ScenarioNetwork).Name);

            //Dokumente für Loesungsbezogene Modelle
            doct = subapp.Documents.Add("");
            //subapp.Documents.OpenEx("SMT_Class.vssx", 4);
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_stP.vsdx"));
            doct.Close();
            pReferencer.AddAssignment(systemname + "_RVP_stP.vsdx", typeof(StrukturellePerspektiveNetwork).Name);

            doct = subapp.Documents.Add("");
            //subapp.Documents.OpenEx("SMT_Activity.vssx", 4);
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_fuP.vsdx"));
            doct.Close();
            pReferencer.AddAssignment(systemname + "_RVP_fuP.vsdx", typeof(FunktionellePerspektiveNetwork).Name);

            doct = subapp.Documents.Add("");
            //subapp.Documents.OpenEx("SMT_SM.vssx", 4);
            doct.SaveAs(System.IO.Path.Combine(path,systemname+  "_RVP_BP.vsdx"));
            doct.Close();
            pReferencer.AddAssignment(systemname + "_RVP_BP.vsdx", typeof(VerhaltensperspektiveNetwork).Name);

        }

        public void SetHyperlink()
        {
            //NUr verwenden, wenn noch keine Hyperlinks gesetzt sind
            IVPage overview = new IVPage();
            foreach (IVPage p in this._application.ActiveDocument.Pages)
            {
                if (p.Name=="System Overview")
                {
                    overview = p;
                }
            }
            foreach (IVShape s in overview.Shapes)
            {
                foreach (IVPage p in this._application.ActiveDocument.Pages)
                {
                    if (s.Text == p.Name)
                    {
                        IVHyperlink hl = s.Hyperlinks.Add();
                        hl.SubAddress = p.Name;

                    }
                }
            }

        }
        public void FunctiontoPage()
        {
            List<IVShape> shapes = new List<IVShape>();
            foreach (IVShape shape in this._application.ActivePage.Shapes)
            {
                if (shape.Name.Contains("Context Function") )
                {
                    bool exists = false;
                    foreach (var s in shapes)
                    {
                        if (s.Text == shape.Text) { exists = true; System.Windows.Forms.MessageBox.Show(shape.Text +
                            " already exists"); }
                    }
                    if (exists == false) { shapes.Add(shape); }
                }

            }
            if (shapes.Count >= 1)
            {
                IVDocument stencil = this._application.Documents.OpenEx("SMT_BeC.vssx", 4);
                IVMaster masterfunction = new IVMaster();
                foreach (var m in stencil.Masters)
                {
                    if (m.Name == "Context Entity/ Function")
                    {
                        masterfunction = m;
                    }
                }
                foreach (var shape in shapes)
                {
                    IVPage page = this._application.ActiveDocument.Pages.Add();
                    IVShape shapeh = page.Drop(masterfunction, 10.3 / 2.54, 20.5 / 2.54);
                    shapeh.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                    
                    //Gruppe auslesen und <<Boundary Name>> umbenennen
                    foreach (var subshape in shapeh.Shapes)
                    {
                        if (subshape.Text == "Context Entity/ Function") { subshape.Text = shape.Text; };
                    }


                    page.Name = shape.Text;
                    IVHyperlink hl = shape.Hyperlinks.Add();
                    hl.SubAddress = page.Name;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No Context Function found.");
            };

        }

        public void EntitytoPage()
        {
            List<IVShape> shapes = new List<IVShape>();
            foreach( IVShape shape in this._application.ActivePage.Shapes)
            {
                if (shape.Name.Contains("Context Entity (CE)"))
                {
                    bool exists = false;
                    foreach (var s in shapes)
                    {
                        if (s.Text==shape.Text) { exists = true; System.Windows.Forms.MessageBox.Show(shape.Text +
                             " already exists.");}
                    }
                    if (exists == false) { shapes.Add(shape); }
                    
                }
                
            }
            if (shapes.Count >= 1)
            {
                foreach (var shape in shapes)
                {
                    IVPage page = this._application.ActiveDocument.Pages.Add();
                    page.Name = shape.Text;
                    IVDocument stencil = this._application.Documents.OpenEx("SMT_BeC.vssx", 4);
                    IVMaster masterentity = new IVMaster();
                    foreach (var m in stencil.Masters)
                    {
                        if (m.Name == "Context Entity/ Function")
                        {
                            masterentity = m;
                        }       
                    }
                    IVShape shapeh = page.Drop(masterentity, 10.3/2.54, 20.5 / 2.54);
                    shapeh.Text = shape.Text;
                    shapeh.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                    IVHyperlink hl = shape.Hyperlinks.Add();
                    hl.SubAddress = page.Name;
                    
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No Context Entity found.");
            };

        }

        public void CreateSubsystems(SPES_DocumentReferencer pReferencer)
        {
            //bestimme Namen des übergeordneten Systems anhand des Seitennamens

            IVPage active = this._application.ActivePage;
            string systemname = active.Name.Substring(4);

            //TODO: check if file is LVP_Overview datei (Logical Viewpoint) / Logical Design
            if(this._application.ActiveDocument.Name != $"{systemname}_LVP.vsdx")
                throw new Exception("Active Document is not the LVP overview file.");

            IVSelection selects = this._application.ActiveWindow.Selection;
            List<IVShape> shapes = new List<IVShape>();

            foreach (var shape in selects.SelectionForDragCopy)
            {
                if (shape.Shapes != null)
                {
                    bool firstshape = true;
                    int count = 0;
                    foreach (var subshape in shape.Shapes)
                    {
                        count++;
                        if (count % 2 == 1)
                        {
                            if (firstshape == true)
                            {
                                shapes.Add(subshape);
                            }
                            firstshape = false;
                        }

                    }
                }
                else
                {
                    shapes.Add(shape);
                }
            }
            //System.Windows.Forms.MessageBox.Show("Create Documents?");
            //getPage "Systemübersicht"--> dazu Document holen mit passender Page
            //speichere aktuelle Applikation ab und suche Applikation mit der Page "Systemübersicht"
            IVDocument systemdoc = null;
            IVPage systemoverview = null;
            Application subapplic = this._application;
            IntPtr subapplickey= new IntPtr(0);
            IntPtr applickey = new IntPtr(0);
            Application applic = null; ;
            bool found = false;

            foreach (var window in OpenWindowGetter.GetOpenWindows())
            {
                if (found == false)
                {
                    if (window.Value.Contains("Visio Professional") || window.Value.Contains("Microsoft Visio"))
                    {
                        OpenWindowGetter.SetForegroundWindow(window.Key);
                        applic = NetOffice.VisioApi.Application.GetActiveInstance();
                        if (subapplic == applic) { subapplickey = window.Key; };
                        foreach (var doc in applic.Documents)
                        {
                            foreach (var page in doc.Pages)
                            {
                                if (page.Name == "System Overview")
                                {
                                    systemdoc = doc;
                                    systemoverview = page;
                                    applickey = window.Key;
                                    found = true;
                                }
                            }
                        }
                    }
                }
            }

            if (found == false)
            {
                var file = new System.IO.DirectoryInfo(
                        new System.IO.FileInfo(_application.ActiveDocument.FullName).Directory.FullName)
                    .GetFiles().First(t => t.Name.Contains("_Overview.vsdx"));
                _application.Documents.Open(file.FullName);

                foreach (var window in OpenWindowGetter.GetOpenWindows())
                {
                    if (found == false)
                    {
                        if (window.Value.Contains("Visio Professional") || window.Value.Contains("Microsoft Visio"))
                        {
                            OpenWindowGetter.SetForegroundWindow(window.Key);
                            applic = NetOffice.VisioApi.Application.GetActiveInstance();
                            if (subapplic == applic) { subapplickey = window.Key; };
                            foreach (var doc in applic.Documents)
                            {
                                foreach (var page in doc.Pages)
                                {
                                    if (page.Name == "System Overview")
                                    {
                                        systemdoc = doc;
                                        systemoverview = page;
                                        applickey = window.Key;
                                        found = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //erstelle für jede ausgewählte Shape/Subsystem auf dem Zeichenblatt "Systemübersicht" ein Rechteck und verbinde dieses mit dem höher gelegenen System
            int counter = 0;
            int sum = shapes.Count;
            IVShape preshape = null;
            foreach (var shape in systemoverview.Shapes)
            {
                if (shape.Text == systemname)
                {
                    preshape = shape;
                }
            }
            double xvalue = (Convert.ToDouble(preshape.CellsSRC(1, 1, 0).FormulaU.Substring(0, preshape.CellsSRC(1, 1, 0).FormulaU.IndexOf(' '))))/10;
            double yvalue = (Convert.ToDouble(preshape.CellsSRC(1, 1, 1).FormulaU.Substring(0, preshape.CellsSRC(1, 1, 1).FormulaU.IndexOf(' '))))/10;
           
            foreach (var shape in shapes)
            {

                IVShape subsystem = systemoverview.DrawRectangle(1, 1, 3, 1.5);
                subsystem.Text = shape.Text;
                subsystem.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                subsystem.SetCenter(BerechneXPosition(xvalue, sum, counter)/2.54, (yvalue - 3.0)/2.54);
                counter++;
                //verbinde zu übergeordnetem System
                preshape.AutoConnect(subsystem, 0);
                //erstelle neues Zeichenblatt und erstelle Hyperlink
                IVPage shapePage = systemdoc.Pages.Add(); 
                shapePage.Name = shape.Text;
                IVHyperlink hl = subsystem.Hyperlinks.Add();
                hl.SubAddress = shapePage.Name; //geht nur wenn, Page in selber Dokumentebene ist.

                //rufe Methode auf, die für die gespeicherten Pages, die benötigten Dokumente erstellt und einbindet
                CreateSubSystemElements(shapePage, applickey, pReferencer);
            }
            //setze Verbinder als gerade/straight
            foreach (var connects in systemoverview.Shapes)
            {
                if (connects.Name.Contains("Dynamic connector") || connects.Name.Contains("Dynamischer Verbinder"))
                {
                    connects.CellsSRC(1, 23, 10).Formula = "16";
                }

            }

            //systemoverview.CreateSelection(VisSelectionTypes.visSelTypeAll).Align(VisHorizontalAlignTypes.visHorzAlignLeft, VisVerticalAlignTypes.visVertAlignMiddle) ;
            OpenWindowGetter.SetForegroundWindow(subapplickey);
        }

        private void CreateSubSystemElements(IVPage p, IntPtr appkey, SPES_DocumentReferencer pReferencer)
        {
            using (Application app = new Application())
            {
                Application subapplic = this._application;
                IntPtr helpappkey = new IntPtr(0);
                Application applic = null; ;

                foreach (var window in OpenWindowGetter.GetOpenWindows())
                {
                    if (window.Value.Contains("Visio Professional"))
                    {
                        OpenWindowGetter.SetForegroundWindow(window.Key);
                        applic = NetOffice.VisioApi.Application.GetActiveInstance();
                        if (app == applic) { helpappkey = window.Key;};
                    };

                }
                OpenWindowGetter.SetForegroundWindow(helpappkey);
                CreateemptyModels(app, this._application.ActiveDocument.Path, p.Name, pReferencer);
                IVShape header, systemName, rvp, fvp, lvp, tvp, statusRvp, statusFvp, statusLvp, statusTvp;
                IVHyperlink rvphl, fvphl, lvphl, tvphl;
                header = p.DrawRectangle(1, 1, 8, 1.5); header.LineStyle = "none"; header.Text = "Artifacts of " + p.Name;
                header.SetCenter(4, (28 / 2.54)); header.CellsSRC(3, 0, 7).FormulaU = "24 pt";
                header.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";

                systemName = p.DrawRectangle(1, 1, 8, 4); systemName.Text = p.Name; systemName.SetCenter(4, (23.2 / 2.54));
                systemName.CellsSRC(1, 11, 4).Formula = "0"; systemName.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";

                rvp = p.DrawRectangle(1, 1, 2.5, 3); rvp.Text = "Requirements Engineering Viewpoint";
                rvp.SetCenter(4.2 / 2.54, (22.8 / 2.54)); rvp.CellsSRC(1, 11, 4).Formula = "0";
                rvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                statusRvp = p.DrawOval(1, 1, 1.16, 1.16); statusRvp.SetCenter(4.2 / 2.54, 23.5 / 2.54); statusRvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

                fvp = p.DrawRectangle(1, 1, 2.5, 3); fvp.Text = "Functional Viewpoint"; fvp.SetCenter(8.2 / 2.54, (22.8 / 2.54));
                fvp.CellsSRC(1, 11, 4).Formula = "0"; fvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                statusFvp = p.DrawOval(1, 1, 1.16, 1.16); statusFvp.SetCenter(8.2 / 2.54, 23.5 / 2.54); statusFvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

                lvp = p.DrawRectangle(1, 1, 2.5, 3); lvp.Text = "Logical Viewpoint"; lvp.SetCenter(12.2 / 2.54, (22.8 / 2.54));
                lvp.CellsSRC(1, 11, 4).Formula = "0"; lvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                statusLvp = p.DrawOval(1, 1, 1.16, 1.16); statusLvp.SetCenter(12.2 / 2.54, 23.5 / 2.54); statusLvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

                tvp = p.DrawRectangle(1, 1, 2.5, 3); tvp.Text = "Technical Viewpoint"; tvp.SetCenter(16.2 / 2.54, (22.8 / 2.54));
                tvp.CellsSRC(1, 11, 4).Formula = "0"; tvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                statusTvp = p.DrawOval(1, 1, 1.16, 1.16); statusTvp.SetCenter(16.2 / 2.54, 23.5 / 2.54); statusTvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

                var doc = app.Documents.Add("");
                CreateRvp(p.Name, doc);
                doc.SaveAs(System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_RVP.vsdx"));
                    
                doc.Close();
                rvphl = rvp.AddHyperlink();
                rvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_RVP.vsdx"));

                doc = app.Documents.Add("");
                CreateFvp(p.Name, doc);
                //todo: load shapes -> load modules
                //app.Documents.OpenEx("SMT_FN_Funktionsnetz.vssx", 4);
                //app.Documents.OpenEx("SMT_IA.vssx", 4);
                doc.SaveAs(System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_FVP.vsdx"));
                doc.Close();
                pReferencer.AddAssignment(systemName.Text + "_FVP.vsdx", typeof(FunktionsnetzNetwork).Name);
                fvphl = fvp.AddHyperlink();
                fvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_FVP.vsdx"));

                doc = app.Documents.Add("");
                CreateLvp(p.Name, doc);
                //app.Documents.OpenEx("SMT_Class.vssx", 4);
                doc.SaveAs(System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_LVP.vsdx"));
                doc.Close();
                pReferencer.AddAssignment(systemName.Text + "_LVP.vsdx", typeof(LogicalViewpointNetwork).Name);
                lvphl = lvp.AddHyperlink();
                lvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_LVP.vsdx"));

                doc = app.Documents.Add("");
                CreateTvp(p.Name, doc);
                //app.Documents.OpenEx("SMT_SM.vssx", 4);
                //app.Documents.OpenEx("SMT_IA.vssx", 4);
                doc.SaveAs(System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_TVP.vsdx"));
                doc.Close();
                pReferencer.AddAssignment(systemName.Text + "_TVP.vsdx", typeof(TechnicalViewpointNetwork).Name);
                tvphl = tvp.AddHyperlink();
                tvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_TVP.vsdx"));
                app.Quit();
                OpenWindowGetter.SetForegroundWindow(appkey);
            }

        }

        private double BerechneXPosition(double x, int sum, int counter)
        {
            // ermittle Position des Vorgängers
            double xwert = 0;
            int range = 10;
            int distance = 0;
                if (sum-1 >0) { distance = range / (sum - 1); }
            xwert = x + (counter * distance - (x / 2));
            return xwert;
        }

        //createfor each Connection an Entry or Exit Point at the Boundary Shape
        /// <summary>
        /// erstellt input/output knoten am rand des interface automaten
        /// </summary>
        public void CreateInandOutput()
        {
            List<IVPage> pagesBound = new List<IVPage>();
            List<IVShape> cons;
            IVMaster input = new IVMaster();
            IVMaster output = new IVMaster();

            foreach (var item in this.ActiveMasters)
            {
                if (item.Name == "Input")
                    input = item;
                else if (item.Name == "Output")
                    output = item;
            }
            foreach (var item in this._application.ActiveDocument.Pages)
            {
                foreach (var shape in item.Shapes)
                {
                    if (shape.Name.Contains("Interface"))  pagesBound.Add(item);

                }
            }
            foreach (var item in pagesBound)
            {

                
                cons = new List<IVShape>();
                IVShape boundary = new IVShape();
                List<IVShape> deleted = new List<IVShape>(); ;
                foreach (var connects in item.Shapes)
                {
                    if (connects.Name.Contains("Connection"))
                    {
                        bool exists = false;
                        foreach (var c in cons)
                        {
                            if (c.Text == connects.Text)
                                exists = true;
                        }
                        if (exists == false)
                            cons.Add(connects);
                    };
                    if (connects.Name.Contains("Interface"))
                        boundary = connects;
                    if (connects.Name.Contains("Output"))
                        deleted.Add(connects);
                    if (connects.Name.Contains("Input"))
                        deleted.Add(connects);
                }
                foreach (var d in deleted)
                {
                    d.Delete();
                }
                string xs = boundary.CellsSRC(1, 1, 0).FormulaU.Substring(0, boundary.CellsSRC(1, 1, 0).FormulaU.IndexOf(' '));
                string ys = boundary.CellsSRC(1, 1, 1).FormulaU.Substring(0, boundary.CellsSRC(1, 1, 1).FormulaU.IndexOf(' '));
                string ws = boundary.CellsSRC(1, 1, 2).FormulaU.Substring(0, boundary.CellsSRC(1, 1, 2).FormulaU.IndexOf(' '));
                string hs = boundary.CellsSRC(1, 1, 3).FormulaU.Substring(0, boundary.CellsSRC(1, 1, 3).FormulaU.IndexOf(' '));

                double xvalue = (Convert.ToDouble(xs.Replace('.', ','))) / 10;
                double yvalue = (Convert.ToDouble(ys.Replace('.', ','))) / 10;
                double weight = (Convert.ToDouble(ws.Replace('.', ','))) / 10;
                double height = (Convert.ToDouble(hs.Replace('.', ','))) / 10;
                int count = 1;

                foreach (var inout in cons)
                {

                    {
                        if (inout.Text.Contains("?"))
                        {
                            double x = (((xvalue - (weight / 2.05)) + ((weight / (cons.Count + 1)) * count)));
                            double y = (yvalue + (height / 2) + 0.25);
                            IVShape inputshape = item.Drop(input, x / 2.54, y / 2.54);
                            foreach (var g in inputshape.Shapes)
                            {
                                if (g.Text.Contains("Input"))  g.Text = inout.Text.Substring(0, inout.Text.IndexOf("?")); 
                            }
                        }
                        else if (inout.Text.Contains("!"))
                        {
                            double x = (((xvalue - (weight / 2.05)) + ((weight / (cons.Count + 1)) * count)));
                            double y = (yvalue + (height / 2) + 0.25);
                            IVShape outputshape = item.Drop(output, x / 2.54, y / 2.54);
                            foreach (var g in outputshape.Shapes)
                            {
                                if (g.Text.Contains("Output")) g.Text = inout.Text.Substring(0, inout.Text.IndexOf("!"));
                            }
                        }
                    }
                    count++;
                }

            }

        }
        public void verify_CREST_Uncertainty()
        {
            List<String> Errors = new List<string>();
            System.Windows.Forms.MessageBox.Show("Start Verification");
            IVPage activePage = this._application.ActivePage;
            // check if Uncertainty, ObservationPoint, Rationale and ACtivation Condition exists.
            bool uncertainty = false;
            bool observation = false;
            bool rationale = false;
            bool activation = false;
            bool and = false;
            bool relation = false;
            foreach (var s in activePage.Shapes)
            {
                if (s.Name.Contains("Uncertainty"))
                {
                    uncertainty = true;
                }
                else if (s.Name.Contains("Rationale"))
                {
                    rationale = true;
                }
                else if (s.Name.Contains("Observation Point"))
                {
                    observation = true;
                }
                else if (s.Name.Contains("Activation Condition"))
                {
                    activation = true;
                }
                else if (s.Name.Contains("AND-Node"))
                {
                    and = true;
                }
                else if (s.Name.Contains("Relation Node"))
                {
                    relation = true;
                }

            }
            //Add Errors to List
            if (!uncertainty)
            {
                Errors.Add("No Uncertainty exists.");
            }
            if (!rationale)
            {
                Errors.Add("No Rationales exists.");
            }
            if (!observation)
            {
                Errors.Add("No Observation Points exists.");
            }
            if (!activation)
            {
                Errors.Add("No Activation Conditions exists.");
            }
            if (!and)
            {
                Errors.Add("No AND-Node exists.");
            }
            if (!relation)
            {
                Errors.Add("No Relation Node exists.");
            }

            //check causes and amplifies links
            foreach (var s in activePage.Shapes)
            {
                if (s.Name.Contains("Causes Link")|| s.Name.Contains("Amplifies Link"))
                {
                    // erhalte Source und Target

                    string frm = s.Cells("BegTrigger").FormulaU;
                    string to = s.Cells("EndTrigger").FormulaU;
                    string helper1 = frm.Remove(frm.Length - 12);
                    string helper2 = to.Remove(to.Length - 12);
                    string fromname = helper1.Substring(11);
                    string toname = helper2.Substring(11);
                    if (!fromname.Contains("Uncertainty") || !toname.Contains("Uncertainty"))
                    {
                        if (s.Name.Contains("Causes Link"))
                        Errors.Add("Causes Link between the Shapes " + fromname +" " + getTextofShape(fromname,activePage)+ " and " + toname +  " "+ getTextofShape(toname, activePage)+" is invalid");
                        else Errors.Add("Amplifies Link between the Shapes " + fromname + " " + getTextofShape(fromname, activePage) + " and " + toname + " " + getTextofShape(toname, activePage) + " is invalid");
                    }

                }
            }
            //check Effect and Mitigation links -->Effect ausgehend von Uncertainty Mitigation eingehend in Uncertainty
            foreach (var s in activePage.Shapes)
            {
                if ( s.Name.Contains("Effect Link"))
                {
                    // erhalte Source und Target
                    string frm = s.Cells("BegTrigger").FormulaU;
                    string to = s.Cells("EndTrigger").FormulaU;
                    string helper1 = frm.Remove(frm.Length - 12);
                    string helper2 = to.Remove(to.Length - 12);
                    string fromname = helper1.Substring(11);
                    string toname = helper2.Substring(11);
                    if (!fromname.Contains("Uncertainty")) Errors.Add("Source of Effect Links must be an Uncertainty.");
                    else
                    {
                        if (toname.Contains("Activation Condition") ||
                            toname.Contains("Observation Point") ||
                            toname.Contains("Rationale") ||
                            toname.Contains("Uncertainty") ||
                            toname.Contains("Relation Node") ||
                            toname.Contains("AND-Node") ||
                            toname.Contains("OR-Node"))
                            Errors.Add("Link between the Shapes " + fromname + " " + getTextofShape(fromname, activePage) + " and " + toname + " " + getTextofShape(toname, activePage) + " is invalid");
                    }
                }
                else if (s.Name.Contains("Mitigation Link"))
                {
                    // erhalte Source und Target
                    string frm = s.Cells("BegTrigger").FormulaU;
                    string to = s.Cells("EndTrigger").FormulaU;
                    string helper1 = frm.Remove(frm.Length - 12);
                    string helper2 = to.Remove(to.Length - 12);
                    string fromname = helper1.Substring(11);
                    string toname = helper2.Substring(11);
                    if (!toname.Contains("Uncertainty")) Errors.Add("Target of Mitigation Links must be an Uncertainty.");
                    else
                    {
                        if (fromname.Contains("Activation Condition") ||
                            fromname.Contains("Observation Point") ||
                            fromname.Contains("Rationale") ||
                            fromname.Contains("Uncertainty") ||
                            fromname.Contains("Relation Node") ||
                            fromname.Contains("AND-Node") ||
                            fromname.Contains("OR-Node"))
                            Errors.Add("Link between the Shapes " + fromname + " " + getTextofShape(fromname, activePage) + " and " + toname + " " + getTextofShape(toname, activePage) + " is invalid");
                    }
                }
            }
            //checke, ob jede Uncertainty mind. 1 Effect und eine Mitigation Link hat
            foreach (var s in activePage.Shapes)
            {
                if (s.Name.Contains("Uncertainty"))
                {
                    bool effect = checkeffects(s.Name, activePage);
                    bool mitigation = checkmitigations(s.Name, activePage);
                    if (!effect) Errors.Add(s.Name + " " + s.Text + " has no Effect Links.");
                    if (!mitigation) Errors.Add(s.Name + " " + s.Text + " has no Mitigation Links.");
                }
            }
//check Trace links
foreach (var s in activePage.Shapes)
{
    if (s.Name.Contains("Trace Link"))
    {
        // erhalte Source und Target
        string frm = s.Cells("BegTrigger").FormulaU;
        string to = s.Cells("EndTrigger").FormulaU;
        string helper1 = frm.Remove(frm.Length - 12);
        string helper2 = to.Remove(to.Length - 12);
        string fromname = helper1.Substring(11);
        string toname = helper2.Substring(11);
        if (!fromname.Contains("Activation Condition") &&
                !fromname.Contains("Observation Point") &&
                !fromname.Contains("Rationale")) Errors.Add("Source of Trace Links must be Activation Condition, Rationale or Observation Point.");
        else
        {
            if (toname.Contains("Activation Condition") ||
                toname.Contains("Observation Point") ||
                toname.Contains("Rationale") ||
                toname.Contains("Uncertainty") ||
                toname.Contains("Relation Node") ||
                toname.Contains("AND-Node") ||
                toname.Contains("OR-Node"))
                Errors.Add("Trace Link between the Shapes " + fromname + " " + getTextofShape(fromname, activePage) + " and " + toname + " " + getTextofShape(toname, activePage) + " is invalid. It must be connected to a Base Artifact.");
        }
    }
}
            //check Relation Links
            foreach (var s in activePage.Shapes)
            {
                if (s.Name.Contains("Relation Link"))
                {
                    // erhalte Source und Target
                    string frm = s.Cells("BegTrigger").FormulaU;
                    string to = s.Cells("EndTrigger").FormulaU;
                    string helper1 = frm.Remove(frm.Length - 12);
                    string helper2 = to.Remove(to.Length - 12);
                    string fromname = helper1.Substring(11);
                    string toname = helper2.Substring(11);

                    if ((!fromname.Contains("Relation Node") && !toname.Contains("Relation Node"))||
                        (!fromname.Contains("AND-Node") && !toname.Contains("AND-Node"))||
                        (!fromname.Contains("OR-Node") && !toname.Contains("OR-Node")))
                    {
                        if (!(toname.Contains("Activation Condition") ||
                            toname.Contains("Observation Point") ||
                            toname.Contains("Rationale") ||
                            toname.Contains("Uncertainty") ||
                            toname.Contains("Relation Node") ||
                            toname.Contains("AND-Node") ||
                            toname.Contains("OR-Node")) ||
                            !(fromname.Contains("Activation Condition") ||
                            fromname.Contains("Observation Point") ||
                            fromname.Contains("Rationale") ||
                            fromname.Contains("Uncertainty") ||
                            fromname.Contains("Relation Node") ||
                            fromname.Contains("AND-Node") ||
                            fromname.Contains("OR-Node")))
                            Errors.Add("Relation Link between the Shapes: " + fromname + " " + getTextofShape(fromname, activePage) + " and " + toname + " " + getTextofShape(toname, activePage) + "is invalid. Relation Links must be connected between Nodes and Uncertainty Artifacts.");
                        if  ((fromname.Contains("AND-Node") ||
                            fromname.Contains("OR-Node"))||
                            (toname.Contains("AND-Node") ||
                            toname.Contains("OR-Node")))
                            {
                            if ((fromname.Contains("Observation Point") ||
                            fromname.Contains("Uncertainty")) ||
                            (toname.Contains("Observation Point") ||
                            toname.Contains("Uncertainty")))
                              Errors.Add("Relation Link between the Shapes: " + fromname + " " + getTextofShape(fromname, activePage) + " and " + toname + " " + getTextofShape(toname, activePage) + "is invalid. Relation Links between Obseervation Point and Uncertainty must be connected over a Relation Node ");
                        }
                        if ((toname.Contains("Activation Condition") ||
                            toname.Contains("Observation Point") ||
                            toname.Contains("Rationale") ||
                            toname.Contains("Uncertainty")) &&
                            (fromname.Contains("Activation Condition") ||
                            fromname.Contains("Observation Point") ||
                            fromname.Contains("Rationale") ||
                            fromname.Contains("Uncertainty")))
                            Errors.Add("Relation Link between the Shapes: " + fromname + " " + getTextofShape(fromname, activePage) + " and " + toname + " " + getTextofShape(toname, activePage) + "is invalid. Relation Links must be connected between Nodes and Uncertainty Artifacts.");
                    }
                }
             }
            //Relation Node ist mit mind. 1 Uncertainty verbunden
            //durchlaufe Shapes, wenn Relation Shape, dann hole ein und ausgehende Nachrichten
            //wenn bei den ein und ausgehenden Nachrichten mind. einmal Shape "Uncertainty" vorkommt ansonsten Fehler werfen.
            //UNcertainty,Rationale, OP, AC hat mind. 1 Relation LInk ein oder ausgehend und  OP, AC, Rat hat mind. 1 Trace Link
            foreach (var s in activePage.Shapes)
            {
                if (s.Name.Contains("Uncertainty")||
                    s.Name.Contains("Rationale") ||
                    s.Name.Contains("Observation Point") ||
                    s.Name.Contains("Activation Condition"))
                {
                    if (!hasRelationLink(s.Name, activePage)) Errors.Add(s.Name + " "+ s.Text + " has no Relation Links.");
                    if (!s.Name.Contains("Uncertainty"))
                    {
                        if (!hasTraceLink(s.Name, activePage)) Errors.Add(s.Name +" "+ s.Text + " has no outgoing Trace Link.");
                    }

                }
                if (s.Name.Contains("Relation Node"))
                {
                    if (!isconnectedtoUncertainty(s.NameU, activePage))
                        Errors.Add("The Relation Shape " + s.Name +" has no connections to any Uncertainty Shapes.");
                }
                if (s.Name.Contains("Uncertainty"))
                {
                    if (!isconnectedtoRelationNode(s.Name, activePage))
                        Errors.Add("The Uncertainty Shape " + s.Text + " has no connections to any Relation Nodes.");
                }
            }
                

            //OR-AND Node Pruefen, dass mindestens zwei eingehende Links verbunden sind. -> pruefen, ob Summe der ein und ausgehenden Links mindestens 3 ist.

           
            foreach (var shape in activePage.Shapes)
            {
                if (shape.Name.Contains("OR-Node")|| shape.Name.Contains("AND-Node"))
                {
                    int[] count = shape.ConnectedShapes(VisConnectedShapesFlags.visConnectedShapesAllNodes, "");
                    if (count.Count()<3)
                    {
                        if (shape.Name.Contains("OR-Node")) Errors.Add("OR-Node is not necessary");
                        else Errors.Add("AND-Node is not necessary");
                    }
                }
            }


            //Throw exception with all found errors
            string errors = "";
            foreach (var e in Errors)
            {
                errors += e;
                errors += Environment.NewLine;
            }
            if (Errors.Count != 0)
                System.Windows.Forms.MessageBox.Show(errors, "Verification failed!");
            else { System.Windows.Forms.MessageBox.Show("Verification successful!"); }
            
        }

        private string getTextofShape(string name, IVPage activePage)
        {
            string text = "";
            foreach (var s in activePage.Shapes)
            {
                if (s.Name.Equals(name)) text = s.Text;
            }

            return text;
        }

        private bool checkmitigations(string name, IVPage activePage)
        {
            bool exists = false;
            foreach (var s in activePage.Shapes) 
            {
                if (s.Name.Contains("Mitigation Link"))
                {
                    string to = s.Cells("EndTrigger").FormulaU;
                    string helper1 = to.Remove(to.Length - 12);
                    string toname = helper1.Substring(11);
                    if (toname.Contains(name)) exists = true;
                }
            }
            return exists;
        }

        private bool checkeffects(string name, IVPage activePage)
        {
            bool exists = false;
            foreach (var s in activePage.Shapes)
            {
                if (s.Name.Contains("Effect Link"))
                {
                    string frm = s.Cells("BegTrigger").FormulaU;
                    string helper1 = frm.Remove(frm.Length - 12);
                    string fromname = helper1.Substring(11);
                    if (fromname.Contains(name)) exists = true;
                }
            }
            return exists;
        }

        private bool hasTraceLink(String shapename, IVPage page)
        {
            bool exists = false;
            foreach (var s in page.Shapes)
            {
                if (s.Name.Contains("Trace Link"))
                {
                    string frm = s.Cells("BegTrigger").FormulaU;
                    string helper1 = frm.Remove(frm.Length - 12);
                    string fromname = helper1.Substring(11);
                    if (fromname.Contains(shapename)) exists = true;
                }
            }
            return exists;
        }
        private bool hasRelationLink(String shapename, IVPage page)
        {
            bool exists = false;
            foreach (var s in page.Shapes)
            {
                if (s.Name.Contains("Relation Link"))
                {
                    string frm = s.Cells("BegTrigger").FormulaU;
                    string to = s.Cells("EndTrigger").FormulaU;
                    string helper1 = frm.Remove(frm.Length - 12);
                    string helper2 = to.Remove(to.Length - 12);
                    string fromname = helper1.Substring(11);
                    string toname = helper2.Substring(11);
                    if (fromname.Contains(shapename) || toname.Contains(shapename))
                        exists = true;
                }
            }
            return exists;
        }
        private bool isconnectedtoRelationNode(String shapename, IVPage page)
        {
            bool exists = false;
            foreach (var s in page.Shapes)
            {
                if (s.Name.Contains("Relation Link"))
                {
                    string frm = s.Cells("BegTrigger").FormulaU;
                    string to = s.Cells("EndTrigger").FormulaU;
                    string helper1 = frm.Remove(frm.Length - 12);
                    string helper2 = to.Remove(to.Length - 12);
                    string fromname = helper1.Substring(11);
                    string toname = helper2.Substring(11);
                    if (fromname.Contains(shapename) || toname.Contains(shapename))
                    {
                        if (fromname.Contains("Relation Node") || toname.Contains("Relation Node")) exists = true;
                    }
                }
            }
            return exists;
        }
        private bool isconnectedtoUncertainty(String shapename, IVPage page)
        {
            bool exists = false;
            foreach (var s in page.Shapes)
            {
                if (s.Name.Contains("Relation Link"))
                {
                    string frm = s.Cells("BegTrigger").FormulaU;
                    string to = s.Cells("EndTrigger").FormulaU;
                    string helper1 = frm.Remove(frm.Length - 12);
                    string helper2 = to.Remove(to.Length - 12);
                    string fromname = helper1.Substring(11);
                    string toname = helper2.Substring(11);
                    if (fromname.Contains(shapename) || toname.Contains(shapename))
                    {
                        if (fromname.Contains("Uncertainty") || toname.Contains("Uncertainty")) exists = true;
                    }
                }
            }
            return exists;
        }
        public void verify_CREST_FunctionNet()
        {
            List<String> Errors = new List<string>();
            System.Windows.Forms.MessageBox.Show("Start Verification");
            //Check existence of at least one system function, context function and system network function
            IVPage activePage = this._application.ActivePage;
            bool sfexists = false;
            bool cfexists = false;
            bool snfexists = false;

            foreach (var s in activePage.Shapes)
            {
                if (s.Name.Contains("System Function"))
                {
                    sfexists = true;
                }
                else if (s.Name.Contains("Context Function"))
                {
                    cfexists = true;
                }
                else if (s.Name.Contains("System Network Function"))
                {
                    snfexists = true;
                }
            }

            //Add Errors to List
            if (!sfexists)
            {
                Errors.Add("No System Function exists.");
            }
            if (!cfexists)
            {
                Errors.Add("No Context Function exists.");
            }
            if (!snfexists)
            {
                Errors.Add("No System Network Function exists.");
            }

            //Check for each function, if at least one interaction/aggregation is incoming or outgoing
            foreach (var s in activePage.Shapes)
            {
                if (s.Name.Contains("System Function") || s.Name.Contains("Context Function")|| s.Name.Contains("System Network Function"))
                {


                    int[] con = s.ConnectedShapes((VisConnectedShapesFlags)0, "");


                    if (con.Count()<=0)
                    {
                        Errors.Add(s.Name + " "+ s.Text +" has no incoming or outgoing Messages.");
                    }
                    

                }
            }
            //Check: Aggregation only valid between (Stereotype) Collaborative Functions (System/Context) and System Network Functions
            foreach (var s in activePage.Shapes)
            {
                if (s.Name.Contains("Aggregation"))
                {
                    // erhalte Source und Target

                    string frm = s.Cells("BegTrigger").FormulaU;
                    string to = s.Cells("EndTrigger").FormulaU;
                    string helper1 = frm.Remove(frm.Length-12);
                    string helper2 = to.Remove(to.Length - 12);
                    string fromname = helper1.Substring(11);
                    string toname = helper2.Substring(11);

                    if (fromname.Contains("System Network Function"))
                    {
                        if (toname.Contains("System Function") || toname.Contains("Context Function"))
                        {
                            //get Stereotypes of Functions
                            string stereotype = GetStereotype(toname);
                            if (!stereotype.Contains("Collaborative"))
                            {
                                Errors.Add("Aggregation between " + GetName(fromname) + " and " + GetName(toname) + " is invalid.");
                            }
                        }
                        else { Errors.Add("Aggregation between " + GetName(fromname) + " and " + GetName(toname) + " is invalid."); }
                    }
                    else
                    {
                        Errors.Add("Aggregation between " + GetName(fromname) + " and " + GetName(toname) + " is invalid.");
                    }
                                                         
                }
 
            }

            //Check: For each System Function exists a Page with an Interface Automaton
            
            foreach (var s in activePage.Shapes)
            {
                if (s.Name.Contains("System Function"))
                {
                    bool exists = false;
                    foreach (var p in this._application.ActiveDocument.Pages)
                    {
                        if (s.Text == p.Name)
                        {
                            exists = true;
                        }
                    } 
                    if (!exists)
                    {
                        Errors.Add("No Page/Interface Automaton found for System Function " + s.Text);
                    }
                }
            }

            //Check interactions of all system functions are existent in the interface automata with correct sign (!, ?)
            //durchlaufe alle Shapes mit Name "System Function"         
            // durchlaufe für jede Interaction (Shape.Name.Contains("")) und speichere in zwei Listen ein und ausgehende Nachrichten für 
            //die aktuelle SF
            // Rufe Funktion X auf, welche prüft, ob alle ein und ausgehenden Nachrichten in verlinkter Page vorhanden sind
            // auch ob gesetzte Zeichen (!/?) gesetzt sind

            foreach (var s in activePage.Shapes)
            {
               
                if (s.Name.Contains("System Function"))
                {
                    List<String> incoming = new List<string>();
                    List<String> outgoing = new List<string>();
                    foreach (var i in activePage.Shapes)
                    {
                        if (i.Name.Contains("Interaction"))
                        {
                            // erhalte Source und Target

                            string frm = i.Cells("BegTrigger").FormulaU;
                            string to = i.Cells("EndTrigger").FormulaU;
                            string helper1 = frm.Remove(frm.Length - 12);
                            string helper2 = to.Remove(to.Length - 12);
                            string fromname = helper1.Substring(11);
                            string toname = helper2.Substring(11);

                            if (s.Name == fromname)
                            {
                                outgoing.Add(i.Text);
                            }
                            else if (s.Name == toname)
                            {
                                incoming.Add(i.Text);
                            }

                        }
                    }
                    //lösche doppelte Nachrichten aus erstellten Listen incoming und outgoing
                    List<String> uniincoming = new List<string>();
                    List<String> unioutgoing = new List<string>();
                    if (incoming.Count >= 1)
                    {
                        foreach (var inc in incoming)
                        {
                            bool exists = false;
                            foreach (var uniinc in uniincoming)
                            {
                                if (inc == uniinc)
                                {
                                    exists = true;
                                }
                            }
                            if (!exists) { uniincoming.Add(inc); }
                        }
                    }
                    if (outgoing.Count >= 1)
                    {
                        foreach (var outg in outgoing)
                        {
                            bool exists = false;
                            foreach (var uniout in unioutgoing)
                            {
                                if (outg == uniout)
                                {
                                    exists = true;
                                }
                            }
                            if (!exists) { unioutgoing.Add(outg); }
                        }
                    }
                    List<String> helper = new List<string>();
                    helper= CheckMessages(s.Text, uniincoming, unioutgoing);
                    if (helper.Count >= 1)
                    {
                        foreach (var h in helper)
                        {
                            Errors.Add(h);
                        }
                    }
                }

            }

            //Throw exception with all found errors
            string errors = "";
            foreach (var e in Errors)
            {
                errors += e;
                errors += Environment.NewLine;
            }
            if (Errors.Count!=0)
            System.Windows.Forms.MessageBox.Show(errors,"Verification failed!");
            else { System.Windows.Forms.MessageBox.Show("Verification successful!"); }
        }

        private string GetStereotype(string name)
        {
            string result = "";
            foreach (var s in this._application.ActivePage.Shapes)
            {
                if (s.Name == name)
                {
                    foreach (var subs in s.Shapes)
                    {
                        if (subs.Name.Contains("Stereotype"))
                            result = subs.Text;
                    }
                }

            }

            return result;
        }


        private string GetName(string name)
        {
            string result = "";
            foreach (var s in this._application.ActivePage.Shapes)
            {
                if (s.Name == name)
                {
                    result = s.Text;
                    
                }

            }

            return result;
        }

        private List<String> CheckMessages(String name,List<String> inc, List<String> outg)
        {
            List<String> faults = new List<string>();
            foreach (var p in this._application.ActiveDocument.Pages)
            {
                if (p.Name == name)
                {
                    List<String> incoming = new List<string>();
                    List<String> outgoing = new List<string>();
                    foreach (var s in p.Shapes)
                    {
                        if (s.Name.Contains("Connection"))
                        {
                            if (s.Text.Contains("?")) { incoming.Add(s.Text); }
                            else if (s.Text.Contains("!")) { outgoing.Add(s.Text); }
                        }
                    }
                    //lösche doppelte Nachrichten aus erstellten Listen incoming und outgoing
                    List<String> uniincoming1 = new List<string>();
                    List<String> unioutgoing1 = new List<string>();
                    if (incoming.Count >= 1)
                    {
                        foreach (var inco in incoming)
                        {
                            bool exists = false;
                            foreach (var uniinc in uniincoming1)
                            {
                                if (inco == uniinc)
                                {
                                    exists = true;
                                }
                            }
                            if (!exists) { uniincoming1.Add(inco); }
                        }
                    }
                    if (outgoing.Count >= 1)
                    {
                        foreach (var outgo in outgoing)
                        {
                            bool exists = false;
                            foreach (var uniout in unioutgoing1)
                            {
                                if (outgo == uniout)
                                {
                                    exists = true;
                                }
                            }
                            if (!exists) { unioutgoing1.Add(outgo); }
                        }
                    }

                    foreach (var ic in inc.ToList())
                    {
                        foreach (string unic in uniincoming1.ToList())
                        {
                            if (ic == unic.Remove(unic.Length-1)) { inc.Remove(ic); uniincoming1.Remove(unic); }
                        }
                    }
                    if (inc.Count >= 1) { faults.Add("Incoming message missing in system function " + name ); }
                    if (uniincoming1.Count >= 1) { faults.Add("Incoming messages missing in interface automaton " + name); }

                    foreach (var og in outg.ToList())
                    {
                        foreach (string unig in unioutgoing1.ToList())
                        {
                            if (og == unig.Remove(unig.Length - 1)) { outg.Remove(og); unioutgoing1.Remove(unig); }
                        }
                    }
                    if (outg.Count >= 1) { faults.Add("Outgoing message missing in System Function " + name ); }
                    if (unioutgoing1.Count >= 1) { faults.Add("Outgoing message missing in System Function  Interface Automaton " + name); }
                }
            }
            return faults;
        }
    }

}

