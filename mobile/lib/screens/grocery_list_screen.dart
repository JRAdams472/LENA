import 'package:flutter/material.dart';

import '../models/grocery_list.dart';
import '../services/api_service.dart';

class GroceryListScreen extends StatefulWidget {
  final ApiService apiService;

  const GroceryListScreen({
    super.key,
    required this.apiService,
  });

  @override
  State<GroceryListScreen> createState() => _GroceryListScreenState();
}

class _GroceryListScreenState extends State<GroceryListScreen> {
  final _idController = TextEditingController();
  GroceryList? _list;
  bool _isLoading = false;
  bool _isSaving = false;
  String? _error;

  Future<void> _loadList() async {
    final idText = _idController.text.trim();
    final id = int.tryParse(idText);
    if (id == null) {
      setState(() => _error = 'Enter a valid list ID');
      return;
    }

    setState(() {
      _isLoading = true;
      _error = null;
      _list = null;
    });

    try {
      final list = await widget.apiService.getGroceryList(id);
      setState(() {
        _list = list;
      });
    } catch (e) {
      setState(() => _error = 'Failed to load list: $e');
    } finally {
      setState(() => _isLoading = false);
    }
  }

  Future<void> _save() async {
    if (_list == null) return;

    setState(() => _isSaving = true);

    try {
      final purchaseDate = DateTime.now();
      for (final item in _list!.groceryListItems) {
        if (item.isChecked && item.itemID != null) {
          await widget.apiService.adjustItemQuantity(
            item.itemID!,
            item.quantityNeeded,
            purchaseDate: purchaseDate,
          );
        }
      }

      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Stock updated')),
        );
      }
    } catch (e) {
      setState(() => _error = 'Failed to save: $e');
    } finally {
      setState(() => _isSaving = false);
    }
  }

  void _toggleItem(GroceryListItem item, bool? value) {
    if (value == null) return;
    setState(() {
      item.isChecked = value;
    });
  }

  @override
  void dispose() {
    _idController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Grocery List')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [
            Row(
              children: [
                Expanded(
                  child: TextField(
                    controller: _idController,
                    keyboardType: TextInputType.number,
                    decoration: const InputDecoration(
                      labelText: 'List ID',
                      hintText: 'Enter grocery list ID',
                    ),
                  ),
                ),
                const SizedBox(width: 8),
                ElevatedButton(
                  onPressed: _isLoading ? null : _loadList,
                  child: const Text('Load'),
                ),
              ],
            ),
            if (_error != null)
              Padding(
                padding: const EdgeInsets.only(top: 8.0),
                child: Text(
                  _error!,
                  style: const TextStyle(color: Colors.red),
                ),
              ),
            const SizedBox(height: 16),
            if (_isLoading)
              const Expanded(child: Center(child: CircularProgressIndicator()))
            else if (_list == null)
              const Expanded(
                child: Center(child: Text('Load a grocery list to begin')),
              )
            else
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Generated: ${_list!.generatedDate}',
                      style: Theme.of(context).textTheme.titleSmall,
                    ),
                    const SizedBox(height: 8),
                    Expanded(
                      child: ListView.builder(
                        itemCount: _list!.groceryListItems.length,
                        itemBuilder: (context, index) {
                          final item = _list!.groceryListItems[index];
                          return CheckboxListTile(
                            title: Text(item.displayName),
                            subtitle: item.itemID == null
                                ? const Text('Manual item (skipped)')
                                : Text(
                                    'Qty: ${item.quantityNeeded} ${item.unitOfMeasure ?? ''}'),
                            value: item.isChecked,
                            onChanged: item.itemID == null
                                ? null
                                : (value) => _toggleItem(item, value),
                          );
                        },
                      ),
                    ),
                    ElevatedButton.icon(
                      onPressed: _isSaving ? null : _save,
                      icon: _isSaving
                          ? const SizedBox(
                              width: 16,
                              height: 16,
                              child: CircularProgressIndicator(strokeWidth: 2),
                            )
                          : const Icon(Icons.save),
                      label: const Text('Save and update stock'),
                    ),
                  ],
                ),
              ),
          ],
        ),
      ),
    );
  }
}
