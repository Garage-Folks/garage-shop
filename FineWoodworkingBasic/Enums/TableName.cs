using System.ComponentModel;

namespace FineWoodworkingBasic.Enums
{
    public enum TableName
    {
        [Description("Authorized User")]
        AuthorizedUser,
        Brand,
        [Description("Constraint of Location")]
        ConstraintOfLocation,
        Glue,
        Location,
        [Description("Location Constraint")]
        LocationConstraint,
        Log,
        Lumber,
        [Description("Misc. Finish Product")]
        MiscFinishProduct,
        [Description("Misc. Wood")]
        MiscWood,
        Oil,
        Paint,
        [Description("Sheet Material")]
        SheetMaterial,
        [Description("Species Wood")]
        SpeciesWood,
        Tool,
        Varnish
    }
}
