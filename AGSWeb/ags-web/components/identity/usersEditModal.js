import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Form, FormGroup, Label, Input, FormText, Badge } from 'reactstrap';
import { ListGroup, ListGroupItem } from 'reactstrap';
import React, { useContext, useEffect, useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTimes } from '@fortawesome/free-solid-svg-icons'
import "../../styles/identity/userEditModal.css"

export default function UsersEditModal({ editData, setEditData, groups }) {
    const isCreate = editData == null ? true : false;
    
    
    const [filererGroupsValue, setFilererGroupsValue] = useState("");
    const [filteredGroups, setFilteredGroups] = useState(groups);
    const [showGroupOptions, setShowGroupOptions] = useState(false);

    useEffect(() => {
        console.log(editData)
    }, [editData])

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
                            Username:
                        </Label>
                        <Input type="text" name="username" placeholder="Username" value={editData.username} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            Email:
                        </Label>
                        <Input type="email" name="email" placeholder="Email" value={editData.email} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            First Name:
                        </Label>
                        <Input type="text" name="first_Name" placeholder="First Name" value={editData.first_Name} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            Last Name:
                        </Label>
                        <Input type="text" name="last_Name" placeholder="Last Name" value={editData.last_Name} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            Title:
                        </Label>
                        <Input type="text" name="title" placeholder="Title" value={editData.title} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            Groups:
                        </Label>
                        <div class="form-control user-edit groups">
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
                                    <ListGroup className={"groupList " + (showGroupOptions ? "" : "hide")}>
                                        {
                                            filteredGroups.map(x => {
                                                return (
                                                    <ListGroupItem className="groupOption" action onClick={() => onGroupOptionClick(x.id)}>
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
            <Button className="groupBadge" color="secondary" type="button">{group.name} <FontAwesomeIcon icon={faTimes} onClick={() => onGroupBadgeRemoveClick(group.id)}></FontAwesomeIcon></Button>
        </React.Fragment>
    )
}