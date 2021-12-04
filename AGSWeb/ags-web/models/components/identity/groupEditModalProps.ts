import { FunctionClaimItemType } from "../../identity/functionClaimItemType";
import { GroupItemType } from "../../identity/groupItemType";

export interface GroupEditModalProps 
{
    editData: GroupItemType
    setEditData: React.Dispatch<GroupItemType>
    functionClaims: FunctionClaimItemType[]
}