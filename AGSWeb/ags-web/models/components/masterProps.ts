import { Context } from "react";
import { AGSContextModel } from "../../helpers/common/agsContext";

export interface MasterProps
{
    children: React.ReactNode
    agsContext: AGSContextModel
}