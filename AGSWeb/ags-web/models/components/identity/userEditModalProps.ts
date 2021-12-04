import { GroupItemType } from "../../identity/groupItemType";
import { UserItemType } from "../../identity/userItemType";

export interface UserEditModalProps
{
    editData: UserItemType
    setEditData: React.Dispatch<UserItemType>
    groups: GroupItemType[]
}