import { Button, ModalBody } from 'reactstrap';
import { Form, FormGroup, Label, Input } from 'reactstrap';
import { ListGroup, ListGroupItem } from 'reactstrap';
import React, { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTimes } from '@fortawesome/free-solid-svg-icons'
import "../../styles/identity/editModal.css"
import { GetLocalizedString } from '../../helpers/common/localizationHelper'
import { UserEditModalProps } from '../../models/components/identity/userEditModalProps';
import { InputEventProps } from '../../models/common/inputEventProps';
import { GroupBadgeProps } from '../../models/components/identity/groupBadgeProps';

export default function UsersEditModal({ editData, setEditData, groups }: UserEditModalProps) {
    const isCreate = editData == null ? true : false;
    
    
    const [filererGroupsValue, setFilererGroupsValue] = useState("");
    const [filteredGroups, setFilteredGroups] = useState(groups);
    const [showGroupOptions, setShowGroupOptions] = useState(false);

    const onUsernameChange = (e: InputEventProps) => {
        editData.username = e.target.value;
        setEditData({ ...editData })
    }

    const onEmailChange = (e: InputEventProps) => {
        editData.email = e.target.value;
        setEditData({ ...editData })
    }

    const onFirstNameChange = (e: InputEventProps) => {
        editData.firstName = e.target.value;
        setEditData({ ...editData })
    }

    const onLastNameChange = (e: InputEventProps) => {
        editData.lastName = e.target.value;
        setEditData({ ...editData })
    }

    const onTitleChange = (e: InputEventProps) => {
        editData.title = e.target.value;
        setEditData({ ...editData })
    }

    const onGroupInputChange = (input: string) => {
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

    const onModalClick = (e: React.MouseEvent<HTMLElement, MouseEvent>) => {
        const groupList = document.getElementById("groupList")
        if (e.target != groupList) {
            setShowGroupOptions(false)
        }
    }

    const onGroupOptionClick = (groupId: string) => {
        editData.groupIds.push(groupId);
        setEditData({ ...editData })
        setShowGroupOptions(false)
        setFilererGroupsValue('')
    }

    const onGroupBadgeRemoveClick = (groupId: string) => {
        const updatedGroupIds: string[] = []; 
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
                        <Input type="text" name="username" placeholder={GetLocalizedString("label_identity_user_table_username")} value={editData.username} onChange={(e) => onUsernameChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                        {GetLocalizedString("label_identity_user_table_email")}:
                        </Label>
                        <Input type="email" name="email" placeholder={GetLocalizedString("label_identity_user_table_email")} value={editData.email} onChange={(e) => onEmailChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                        {GetLocalizedString("label_identity_user_table_first_name")}:
                        </Label>
                        <Input type="text" name="firstName" placeholder={GetLocalizedString("label_identity_user_table_first_name")} value={editData.firstName} onChange={(e) => onFirstNameChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                        {GetLocalizedString("label_identity_user_table_last_name")}:
                        </Label>
                        <Input type="text" name="lastName" placeholder={GetLocalizedString("label_identity_user_table_last_name")} value={editData.lastName} onChange={(e) => onLastNameChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                        {GetLocalizedString("label_identity_user_table_title")}:
                        </Label>
                        <Input type="text" name="title" placeholder="Title" value={editData.title} onChange={(e) => onTitleChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            {GetLocalizedString("label_identity_user_table_groups")}:
                        </Label>
                        <div className="form-control autocomplete">
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

function GroupBadge({group, onGroupBadgeRemoveClick}: GroupBadgeProps) {
    return (
        <React.Fragment>
            <Button className="optionBadge" color="primary" type="button">{group.name} <FontAwesomeIcon icon={faTimes} onClick={() => onGroupBadgeRemoveClick(group.id)}></FontAwesomeIcon></Button>
        </React.Fragment>
    )
}