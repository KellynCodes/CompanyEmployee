﻿namespace Repository.Dtos
{
    [Serializable]
    public record EmployeeDto(Guid Id, string Name, int Age, string Position);

}
