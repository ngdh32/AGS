import GroupsHelper from '../../../../helpers/identity/api/groupsHelper'
import { NextApiRequest, NextApiResponse } from 'next';

// req = HTTP incoming message, res = HTTP server response
export default async function GroupsHandler(req: NextApiRequest, res: NextApiResponse) {
    const groupsHelper = new GroupsHelper(req,res);
    const { method, query } = req;
    const { groupId } = query

    
    if (method == "DELETE" && typeof groupId === 'string'){
        const result = await groupsHelper.DeleteGroup(groupId);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }

}