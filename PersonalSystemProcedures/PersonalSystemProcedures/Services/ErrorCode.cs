using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Services
{
    public enum ErrorCode
    {
        TodoItemNameAndNotesRequired,
        TodoItemIDInUse,
        RecordNotFound,
        CouldNotCreateItem,
        CouldNotUpdateItem,
        CouldNotDeleteItem,
        CouldNotLoginItem,
        CouldNotBuildUser,
        ExceptionError
    }
}
