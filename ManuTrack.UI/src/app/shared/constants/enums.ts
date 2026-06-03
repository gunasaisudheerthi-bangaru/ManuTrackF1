// Mirrors backend enum classes exactly

export const AppRoles = [
  { value: 'Admin',             label: 'Admin' },
  { value: 'Planner',           label: 'Planner' },
  { value: 'Operator',          label: 'Operator' },
  { value: 'Inspector',         label: 'Inspector' },
  { value: 'InventoryManager',  label: 'Inventory Manager' },
  { value: 'ComplianceOfficer', label: 'Compliance Officer' },
];

export const ProductStatuses = [
  { value: 'Draft',        label: 'Draft' },
  { value: 'Active',       label: 'Active' },
  { value: 'Discontinued', label: 'Discontinued' },
];

export const ProductCategories = [
  { value: 'Furniture',    label: 'Furniture' },
  { value: 'Hardware',     label: 'Hardware' },
  { value: 'Storage',      label: 'Storage' },
  { value: 'Industrial',   label: 'Industrial' },
  { value: 'Electronics',  label: 'Electronics' },
];

export const WorkOrderStatuses = [
  { value: 'Pending',    label: 'Pending' },
  { value: 'Scheduled',  label: 'Scheduled' },
  { value: 'InProgress', label: 'In Progress' },
  { value: 'Completed',  label: 'Completed' },
  { value: 'Cancelled',  label: 'Cancelled' },
];

export const MaterialTypes = [
  { value: 'RawMaterial', label: 'Raw Material' },
  { value: 'Part',        label: 'Part' },
  { value: 'SubAssembly', label: 'Sub Assembly' },
  { value: 'Chemical',    label: 'Chemical' },
  { value: 'Consumable',  label: 'Consumable' },
];

export const MaterialUnits = [
  { value: 'pcs',    label: 'pcs (pieces)' },
  { value: 'kg',     label: 'kg (kilograms)' },
  { value: 'metres', label: 'metres' },
  { value: 'litres', label: 'litres' },
  { value: 'grams',  label: 'grams' },
  { value: 'sheets', label: 'sheets' },
  { value: 'rolls',  label: 'rolls' },
];

export const NotificationCategories = [
  { value: 'General',    label: 'General' },
  { value: 'WorkOrder',  label: 'Work Order' },
  { value: 'Inventory',  label: 'Inventory' },
  { value: 'Quality',    label: 'Quality' },
  { value: 'Compliance', label: 'Compliance' },
];

export const NotificationPriorities = [
  { value: 'Low',      label: 'Low' },
  { value: 'Medium',   label: 'Medium' },
  { value: 'High',     label: 'High' },
  { value: 'Critical', label: 'Critical' },
];
