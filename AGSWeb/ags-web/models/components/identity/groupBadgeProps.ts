import { GroupItemType } from "../../identity/groupItemType";

export interface GroupBadgeProps
{
    group: GroupItemType
    onGroupBadgeRemoveClick: (groupId: string) => void
}