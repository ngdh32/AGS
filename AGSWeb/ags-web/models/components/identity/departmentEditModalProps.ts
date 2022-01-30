import { DepartmentItemType } from "../../identity/departmentItemType";
import { UserItemType } from "../../identity/userItemType";

export interface DepartmentditModalProps
{
    editData: DepartmentItemType
    setEditData: React.Dispatch<DepartmentItemType>
    users: UserItemType[]
}