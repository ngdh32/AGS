export interface MenuItemType
{
    id: string
    labelKey: string
    functionClaim: string
    url: string
    childrenMenus?: MenuItemType[]
}