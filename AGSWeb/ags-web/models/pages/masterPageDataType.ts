import { AGSContextModel } from "../../helpers/common/agsContext";
import { PageDataType } from "./pageDataType";

export interface MasterPageDataType extends PageDataType{
    agsContext: AGSContextModel
}