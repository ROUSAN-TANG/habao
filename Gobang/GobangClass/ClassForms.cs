using System;
using System.Windows.Forms;

namespace GobangClass
{
    class ClassForms : System.Collections.CollectionBase 
    {
        public ClassForms()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        public void add(Form f)
        {
            base.InnerList.Add(f);
        }

        public void Romove(Form f)
        {
            base.InnerList.Remove(f);
        }

        public Form this[int index]
        {
            get
            {
                return ((Form)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }
    }
}
