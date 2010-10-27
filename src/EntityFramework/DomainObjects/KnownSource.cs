namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;

    public partial class KnownSource: IKnownSource
    {
        public KnownSourceGrade Grade_
        {
            get
            {
                return (KnownSourceGrade)Grade;
            }
            set
            {
                Grade = (int) value;
            }
        }
    }
}
