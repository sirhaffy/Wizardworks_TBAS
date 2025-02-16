output "resource_group_name" {
  description = "Name of the created resource group"
  value       = azurerm_resource_group.main.name
}

output "vm_public_ip" {
  description = "Public IP address of the VM"
  value       = azurerm_public_ip.main.ip_address
}

output "vm_name" {
  description = "Name of the created VM"
  value       = azurerm_linux_virtual_machine.main.name
}

output "vm_fqdn" {
  description = "FQDN of the VM"
  value       = azurerm_public_ip.main.fqdn
}

output "ssh_command" {
  description = "Command to SSH into the VM"
  value       = "ssh ${var.admin_username}@${azurerm_public_ip.main.ip_address}"
}

output "application_url" {
  description = "URL to access the application"
  value       = "http://${azurerm_public_ip.main.ip_address}"
}

output "deployment_version" {
  value = var.deployment_timestamp
}