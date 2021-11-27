using System;

namespace Challenge.Core.Entity
{
    public interface ITrackable
    {
        DateTime? CreatedDateTime { get; set; }
        DateTime? UpdatedDateTime { get; set; }
    }
}