import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Form, FormGroup, Label, Input, FormText, Badge } from 'reactstrap';
import { ListGroup, ListGroupItem } from 'reactstrap';
import React, { useContext, useEffect, useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTimes } from '@fortawesome/free-solid-svg-icons'
import "../../styles/identity/editModal.css"
import { GetLocalizedString } from '../../helpers/common/localizationHelper.js'

export default function UsersEditModal({ editData, setEditData, groups }) {
    const isCreate = editData == null ? true : false;
    
    
    const [filererGroupsValue, setFilererGroupsValue] = useState("");
    const [filteredGroups, setFilteredGroups] = useState(groups);
    const [showGroupOptions, setShowGroupOptions] = useState(false);

    const onValueChange = (e) => {
        editData[e.target.getAttribute("name")] = e.target.value;
        setEditData({ ...editData })
    }

    const onGroupInputChange = (input) => {
        setFilererGroupsValue(input);

        let groupOptions = input == null || input == "" ? groups : groups.filter(x => {
            return x.name.toUpperCase().includes(input.toUpperCase());
        });

        groupOptions = groupOptions.filter(x => {
            const isAdded = editData.groupIds.some(y => {
                return y == x.id
            })
            return isAdded ? false : true;
        })

        setFilteredGroups(groupOptions);
        setTimeout(() => setShowGroupOptions(true), 0);
    }

    const onModalClick = (e) => {
        const groupList = document.getElementById("groupList")
        if (e.target != groupList) {
            setShowGroupOptions(false)
        }
    }

    const onGroupOptionClick = (groupId) => {
        editData.groupIds.push(groupId);
        setEditData({ ...editData })
        setShowGroupOptions(false)
        setFilererGroupsValue('')
    }

    const onGroupBadgeRemoveClick = (groupId) => {
        const updatedGroupIds = []; 
        editData.groupIds.forEach(x => {
            if (x != groupId){
                updatedGroupIds.push(x);
            }
        })
        console.log(updatedGroupIds)
        
        editData.groupIds = updatedGroupIds;
        setEditData({...editData})
    }

    const selectedGroupBadges = editData.groupIds == null ? (<React.Fragment></React.Fragment>) : editData.groupIds.map(x => {
        const group = groups.find(y => {return y.id == x});
        return (
            <GroupBadge group={group} onGroupBadgeRemoveClick={onGroupBadgeRemoveClick}/>
        )
    })

    return (
        <React.Fragment>
            <ModalBody onClick={onModalClick}>
                <Form>
                    <FormGroup>
                        <Label>
                            {GetLocalizedString("label_identity_user_table_username")}:
                        </Label>
                        <Input type="text" name="username" placeholder={GetLocalizedString("label_identity_user_table_username")} value={editData.username} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                        {GetLocalizedString("label_identity_user_table_email")}:
                        </Label>
                        <Input type="email" name="email" placeholder={GetLocalizedString("label_identity_user_table_email")} value={editData.email} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                        {GetLocalizedString("label_identity_user_table_first_name")}:
                        </Label>
                        <Input type="text" name="first_Name" placeholder={GetLocalizedString("label_identity_user_table_first_name")} value={editData.first_Name} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                        {GetLocalizedString("label_identity_user_table_last_name")}:
                        </Label>
                        <Input type="text" name="last_Name" placeholder={GetLocalizedString("label_identity_user_table_last_name")} value={editData.last_Name} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                        {GetLocalizedString("label_identity_user_table_title")}:
                        </Label>
                        <Input type="text" name="title" placeholder="Title" value={editData.title} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            {GetLocalizedString("label_identity_user_table_groups")}:
                        </Label>
                        <div class="form-control autocomplete">
                            {
                                selectedGroupBadges
                            }
                            <div>
                                <Input className="input" type="text" 
                                    onClick={() => onGroupInputChange(null)} 
                                    onChange={(e) => { onGroupInputChange(e.target.value) }}
                                    value={filererGroupsValue}
                                    >    
                                </Input>
                                <div id="groupList">
                                    <ListGroup className={"optionList " + (showGroupOptions ? "" : "hide")}>
                                        {
                                            filteredGroups.map(x => {
                                                return (
                                                    <ListGroupItem className="option" action onClick={() => onGroupOptionClick(x.id)}>
                                                        {x.name}
                                                    </ListGroupItem>
                                                )
                                            })
                                        }
                                    </ListGroup>
                                </div>
                            </div>
                        </div>
                    </FormGroup>
                </Form>
            </ModalBody>
        </React.Fragment>
    )
}

function GroupBadge({group, onGroupBadgeRemoveClick}) {
    return (
        <React.Fragment>
            <Button className="optionBadge" color="primary" type="button">{group.name} <FontAwesomeIcon icon={faTimes} onClick={() => onGroupBadgeRemoveClick(group.id)}></FontAwesomeIcon></Button>
        </React.Fragment>
    )
}