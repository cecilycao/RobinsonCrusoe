<<<<<<< Updated upstream
﻿
public interface INegativeResourceCollector :IInteractable
{
    int ResourceAccount_buildingMat { get; }
    int ResourceAccount_foodMat { get; }
}
=======
﻿
public interface IBuildingResourceCollector :IInteractable
{
    int ResourceAccount { get; }
}
>>>>>>> Stashed changes
