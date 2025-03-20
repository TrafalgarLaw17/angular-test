import { Component, OnInit } from '@angular/core';
import { PermissionService } from '../../services/permissions.service';

@Component({
  selector: 'app-permissions',
  templateUrl: './permissions.component.html',
  styleUrls: ['./permissions.component.css']
})
export class PermissionsComponent implements OnInit {
  permissionCategories = ['R', 'E', 'A', 'D', 'P']; // Define access levels
  flattenedPermissions: any[] = [];
  userPermissions: Record<string, string[]> = {};
  selectedPermissions: { name: string; access: string[] }[] = [];

  constructor(private permissionService: PermissionService) {}

  ngOnInit(): void {
    this.loadPermissions();
  }

  // 🔄 Load Permissions (Base + User)
  loadPermissions(): void {
    Promise.all([
      this.permissionService.getBasePermissions().toPromise(),
      this.permissionService.getUserPermissions().toPromise()
    ]).then(([baseResponse, userResponse]) => {
      if (baseResponse.success) {
        this.flattenedPermissions = this.flattenPermissions(baseResponse.data, 0);
      }
      if (userResponse.success) {
        this.userPermissions = this.mapUserPermissions(userResponse.data);
      }
      console.log(' Loaded Permissions:', this.userPermissions); // Debugging Log
      this.updateSelectedPermissions();
    }).catch(error => console.error(' Error loading permissions:', error));
  }
  

  // 🛠️ Flatten Permissions Hierarchy
  flattenPermissions(permissions: any[], level: number): any[] {
    let flatPermissions: any[] = [];
    for (let perm of permissions) {
      flatPermissions.push({ ...perm, level });
      if (perm.child && perm.child.length) {
        flatPermissions.push(...this.flattenPermissions(perm.child, level + 1));
      }
    }
    return flatPermissions;
  }

  // 🛠️ Convert User Permissions for Easy Lookup
  mapUserPermissions(userPerms: any[]): Record<string, string[]> {
    let mappedPermissions = userPerms.reduce((acc, { permissionId, permissionAccess }) => {
      acc[permissionId] = (typeof permissionAccess === 'string') ? permissionAccess.split('-') : [];
      return acc;
    }, {} as Record<string, string[]>);
  
    console.log('✅ Mapped User Permissions:', mappedPermissions); // Debugging Log
    return mappedPermissions;
  }
  

  //  Check if a Checkbox Should Be Checked
  isChecked(perm: any, category: string): boolean {
    // If user permissions exist, use them. Otherwise, use the API default.
    return this.userPermissions[perm.permissionId]
      ? this.userPermissions[perm.permissionId].includes(category)
      : perm.permissionAccess.includes(category);
  }
  

  // 🔄 Toggle Checkbox (Restricted Based on Access)
  togglePermission(perm: any, category: string): void {
    if (this.isDisabled(perm, category)) return; // Restrict invalid toggles
  
    const permId = perm.permissionId;
  
    // Ensure there's an array for this permission
    if (!this.userPermissions[permId]) {
      this.userPermissions[permId] = [...perm.permissionAccess.split('-')]; // Clone initial values
    }
  
    // Toggle only the clicked checkbox
    const index = this.userPermissions[permId].indexOf(category);
    if (index > -1) {
      this.userPermissions[permId].splice(index, 1); // Remove category
    } else {
      this.userPermissions[permId] = [...this.userPermissions[permId], category]; // Add category
    }
  
    this.userPermissions = { ...this.userPermissions }; // Ensure UI updates properly
  
    this.updateSelectedPermissions(); // Update UI immediately
  }
  

  // 🛠️ Disable Checkboxes That Are Not in Available Access
  isDisabled(perm: any, category: string): boolean {
    return !perm.permissionAccess.includes(category); //  Disable if not in access list
  }

  //  Update UI with Selected Permissions
  updateSelectedPermissions(): void {
    this.selectedPermissions = Object.entries(this.userPermissions)
      .map(([id, access]) => {
        const perm = this.flattenedPermissions.find(p => p.permissionId == id);
        return perm ? { name: perm.permissionName, access } : null;
      })
      .filter((perm): perm is { name: string; access: string[] } => perm !== null);
  }

  // 🔄 Save Updated Permissions
  updatePermissions(): void {
    let permissionString = Object.entries(this.userPermissions)
      .map(([id, access]) => `${id}~${access.join('-')}`)
      .join('_');

    this.permissionService.updatePermissions(permissionString).subscribe(response => {
      if (response.success) {
        alert('✅ Permissions updated successfully!');
      } else {
        alert('❌ Failed to update permissions.');
      }
    });
  }
}
